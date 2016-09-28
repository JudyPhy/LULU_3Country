// ProtoMsgManager
package main

import proto "code.google.com/p/goprotobuf/proto"

import (
	"fmt"
	"net"
	"testClient/DatabaseManager"
	"testClient/pb"
	"testClient/pbMsgEnum"
)

/*
	根据proto消息ID，解析proto消息
*/
func ProcessProtoMsgByPID(conn *net.TCPConn, packetHeadId int32, packetContent []byte) {
	fmt.Println("\n=====>>>接收到消息：ProcessProtoMsgByPID", packetHeadId)
	switch packetHeadId {
	case pbMsgEnum.C2GASLogin:
		{
			//登录
			fmt.Println("C2GASLogin")
			msg := &pb.C2GASLogin{}
			err := proto.Unmarshal(packetContent, msg) //unSerialize
			checkError(err)
			fmt.Print("read : ")
			fmt.Println(msg.String())
			DatabaseManager.ProcessMsg_C2GASLogin(conn, msg)
		}
	case pbMsgEnum.C2GASRegister:
		{
			//注册
			fmt.Println("C2GASRegister")
			msg := &pb.C2GASRegister{}
			err := proto.Unmarshal(packetContent, msg) //unSerialize
			checkError(err)
			fmt.Print("read : ")
			DatabaseManager.ProcessMsg_C2GASRegister(conn, msg)
		}
	case pbMsgEnum.C2GSHeartTick:
		{
			//心跳
			fmt.Println("C2GSHeartTick")
			DatabaseManager.ProcessMsg_C2GSHeartTick(conn)
		}
	case pbMsgEnum.C2GSRequestRoleInfo:
		{
			//获取账号下角色列表
			fmt.Println("C2GSRequestRoleInfo")
			msg := &pb.C2GSRequestRoleInfo{}
			err := proto.Unmarshal(packetContent, msg) //unSerialize
			checkError(err)
			fmt.Print("read : ")
			DatabaseManager.ProcessMsg_C2GSRequestRoleInfo(conn, msg)
		}
	case pbMsgEnum.C2GSEnterGame:
		{
			//进入游戏
			fmt.Println("C2GSEnterGame")
			msg := &pb.C2GSEnterGame{}
			err := proto.Unmarshal(packetContent, msg) //unSerialize
			checkError(err)
			fmt.Print("read : ")
			DatabaseManager.ProcessMsg_C2GSEnterGame(conn, msg)
		}
	default:
		fmt.Println("default")
	}
}

/*
	封装proto消息，发送
*/
func PackAndSendProtoMsg(packetHeadId int32, packetContent []byte) {
	fmt.Println("PackAndSendProtoMsg")

}
