// EnterGameMsg
package DatabaseManager

import (
	"fmt"
)

/*
	登入游戏时，发送相关消息
*/
fu SendMsgByEnterGame(){
	SendRoleDetailInfo()
}

/*
	发送角色详细信息
*/
func SendRoleDetailInfo(packetHeadId int32, packetContent []byte) {
	fmt.Println("SendRoleDetailInfo")
	/*
		retMsg := &pb.GS2CRequestRoleInfoRet{}
		retMsg.AccountName = proto.String(msg.GetAccountName())
		for i := 0; i < len(list); i++ {
			retMsg.RoleInfoList[i] = &list[i]
		}
		fmt.Println("RoleInfoList Len:", len(retMsg.RoleInfoList))
		sendData, err := proto.Marshal(retMsg)
		if err != nil {
			fmt.Println("marshaling error: ", err)
		}
		ProcessRetMsg(conn, sendData, (int32)(pbMsgEnum.GS2CRequestRoleInfoRet))
	*/
}
