// RetMsgManager
package DatabaseManager

import (
	"bytes"
	"encoding/binary"
	"fmt"
	"net"
)

type RetMsg struct {
	Length int32
	Msg    []byte
}

/*
   封装准备发送给客户端的消息
*/
func ProcessRetMsg(conn *net.TCPConn, retMsg []byte, msgId int32) {
	//包头前4个字节：消息长度（包头+内容）
	var pachetHead_0_4 []byte = make([]byte, 4)
	buf_head_0_4 := bytes.NewBuffer(pachetHead_0_4)
	var contentLen int32
	binary.Read(buf_head_0_4, binary.BigEndian, &contentLen)
	contentLen = (int32)(len(retMsg) + 8)
	binary.Write(buf_head_0_4, binary.BigEndian, contentLen)
	//fmt.Println("contentLen : ", contentLen, "  buf_head_0_4 : ", buf_head_0_4.Bytes())
	//包头后4个字节：消息PID
	var pachetHead_4_8 []byte = make([]byte, 4)
	buf_head_4_8 := bytes.NewBuffer(pachetHead_4_8)
	var pid int32
	binary.Read(buf_head_4_8, binary.BigEndian, &pid)
	pid = msgId
	binary.Write(buf_head_4_8, binary.BigEndian, pid)
	//fmt.Println("msgId : ", msgId, "  buf_head_4_8 : ", buf_head_4_8.Bytes())
	//组合包头和内容
	buf_result := [][]byte{buf_head_0_4.Bytes(), buf_head_4_8.Bytes(), retMsg}
	sep := []byte("")
	msgRet := bytes.Join(buf_result, sep)
	//fmt.Println("发送消息到客户端 msgret : ", msgRet)
	SendMsg(conn, contentLen, msgRet)
}

func SendMsg(conn *net.TCPConn, contentLen int32, msgRet []byte) (n int, err error) {
	fmt.Println("\n发送消息到客户端")
	addr := conn.RemoteAddr().String()
	n, err = conn.Write(msgRet)
	if err == nil {
		fmt.Println("=>", addr, string(msgRet))
	}
	return
}
