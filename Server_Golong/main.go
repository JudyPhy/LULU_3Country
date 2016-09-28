package main

import (
	"bytes"
	"encoding/binary"
	"fmt"
	"net"
	"os"
	"strconv"
	"strings"
	"sync"
	"time"
)

func main() {
	tcpStart(10113)
}

/*
   定义相关锁
*/
var (
	connMkMutex    sync.Mutex
	connDelMutex   sync.Mutex
	PacketHeadSize int = 8
)

/*
   定义socket conn 映射
*/
var clisConnMap map[string]*net.TCPConn

/*
   建立socket conn 映射
*/
func mkClisConn(key string, conn *net.TCPConn) {
	connMkMutex.Lock()
	defer connMkMutex.Unlock()
	clisConnMap[key] = conn
}

/*
   删除socket conn 映射
*/
func delClisConn(key string) {
	connDelMutex.Lock()
	defer connDelMutex.Unlock()
	delete(clisConnMap, key)
}

/*
   初始化socket conn 映射
*/
func InitClisConnMap() {
	clisConnMap = make(map[string]*net.TCPConn)
}

/*
   启动服务
*/
func tcpStart(port int) {
	fmt.Println("tcpStart")
	InitClisConnMap()
	host := "127.0.0.1:" + strconv.Itoa(13131) //数字转字符串
	//将一个host地址转换为TCPAddr。host=ip:port
	tcpAddr, err := net.ResolveTCPAddr("tcp4", host)
	checkError(err)
	//搭建一个TCP Server
	listener, err := net.ListenTCP("tcp", tcpAddr)
	checkError(err)
	fmt.Println("启动监听，等待链接！IP[", tcpAddr.IP, "]  Port[", tcpAddr.Port, "]\n")
	for {
		conn, err := listener.AcceptTCP() //接收端口任意请求，只有接收到客户端发送的消息时，才会继续执行后面的代码
		if err != nil {
			continue
		}
		fmt.Println("客户端已连接！\n")
		go handleClient(conn)
	}
}

/*
   socket conn 解析接收到的消息
*/
func handleClient(conn *net.TCPConn) {
	SaveConnection(conn)
	request := make([]byte, 1024)
	defer conn.Close() //函数返回时执行这里
	for {
		setReadTimeout(conn, 10*time.Minute) //超时处理
		read_len, err := conn.Read(request)  //读取消息到request中
		if err != nil {
			fmt.Println("ERR:", "read err", err.Error())
			break
		}
		//fmt.Println("read_len", read_len)
		if read_len == 0 {
			fmt.Println("ERR:", "msg readed is nil")
			break
		} else {
			//消息长度
			packetLengthStr := request[:4]
			var packetLength int32
			buf1 := bytes.NewBuffer(packetLengthStr)
			binary.Read(buf1, binary.LittleEndian, &packetLength)
			//fmt.Println("packetLength", packetLength)
			//消息ID
			packetHeadIdStr := request[4:8]
			var packetHeadId int32
			buf2 := bytes.NewBuffer(packetHeadIdStr)
			binary.Read(buf2, binary.LittleEndian, &packetHeadId)
			//fmt.Println("packetHeadId", packetHeadId)

			ProcessProtoMsgByPID(conn, packetHeadId, request[8:packetLength])
		}
	}
	//解析完消息，断开连接，防止粘包
	connectionLost(conn)
}

/*
   存储连接地址
*/
func SaveConnection(conn *net.TCPConn) {
	addr := conn.RemoteAddr().String()
	ip := strings.Split(addr, ":")[0]
	mkClisConn(ip, conn) //存到map里
	fmt.Println("connectionMade:", addr)
}

/*
   设置读数据超时
*/
func setReadTimeout(conn *net.TCPConn, t time.Duration) {
	conn.SetReadDeadline(time.Now().Add(t))
}

/*
   删除连接映射
*/
func connectionLost(conn *net.TCPConn) {
	fmt.Println("删除连接映射 connectionLost")
	addr := conn.RemoteAddr().String()
	ip := strings.Split(addr, ":")[0]
	delClisConn(ip) // 删除关闭的连接对应的clisMap项
	fmt.Println("connectionLost:", addr)
}

/*
   错误处理
*/
func checkError(err error) {
	if err != nil {
		fmt.Println("checkError:", err.Error())
		os.Exit(1)
	}
}
