package DatabaseManager

import proto "code.google.com/p/goprotobuf/proto"

import (
	"database/sql"
	"fmt"
	_ "github.com/go-sql-driver/mysql"
	"net"
	"testClient/Table1"
	"testClient/pb"
	"testClient/pbMsgEnum"
)

/* 初始化数据库引擎 */
func GetConnectDB() *sql.DB {
	//fmt.Println("GetConnectDB")
	db, err := sql.Open("mysql", "root:root@/test")
	//第一个参数 ： 数据库引擎
	//第二个参数 : 数据库DSN配置。Go中没有统一DSN,都是数据库引擎自己定义的，因此不同引擎可能配置不同
	if err != nil {
		fmt.Println("database initialize error : ", err.Error())
		return nil
	}
	return db
}

/*
	账号登录
*/
func ProcessMsg_C2GASLogin(conn *net.TCPConn, msg *pb.C2GASLogin) {
	db := GetConnectDB()
	defer db.Close()
	loginStatus := Table1.CheckAccountNameAndPassword(db, msg.GetAccountName(), msg.GetPassword())
	retMsg := &pb.GAS2CLoginRet{}
	retMsg.AccountName = proto.String(msg.GetAccountName())
	switch loginStatus {
	case 0:
		//账号不存在
		fmt.Println("账号不存在")
		retMsg.RetCode = pb.GAS2CLoginRet_LOGIN_ACCOUNTNAME_ERR.Enum()
		break
	case 1:
		//账号密码正确
		fmt.Println("账号密码正确")
		retMsg.RetCode = pb.GAS2CLoginRet_LOGIN_SUCCESS.Enum()
		break
	case 2:
		//获取数据库数据出错
		fmt.Println("获取数据库数据出错")
		retMsg.RetCode = pb.GAS2CLoginRet_LOGIN_FAIL.Enum()
		break
	case 4:
		//密码错误
		fmt.Println("密码错误")
		retMsg.RetCode = pb.GAS2CLoginRet_LOGIN_PWD_ERR.Enum()
		break
	default:
		break
	}
	sendData, err := proto.Marshal(retMsg)
	if err != nil {
		fmt.Println("marshaling error: ", err)
	}
	//fmt.Println("ProcessMsg_C2GASLogin Ret : ", sendData)

	ProcessRetMsg(conn, sendData, (int32)(pbMsgEnum.GAS2CLoginRet))
}

func DecParams(params []string) string {
	if len(params) <= 0 {
		return ""
	}
	paramStr := params[0]
	for i := 1; i < len(params); i++ {
		paramStr += "," + params[i]
	}
	return paramStr
}

/*
	注册账号
*/
func ProcessMsg_C2GASRegister(conn *net.TCPConn, msg *pb.C2GASRegister) {
	db := GetConnectDB()
	defer db.Close()
	loginStatus := Table1.CheckAccountNameAndPassword(db, msg.GetAccountName(), msg.GetPassword())
	fmt.Println("loginStatus", loginStatus)
	retMsg := &pb.GAS2CRegisterRet{}
	retMsg.AccountName = proto.String(msg.GetAccountName())
	switch loginStatus {
	case 0:
		{
			//账号不存在
			fmt.Println("账号不存在")
			tx, err_tx := db.Begin() //事务
			if err_tx != nil {
				fmt.Println("err_tx : ", err_tx.Error())
				return
			}
			stmt, err_stmt := tx.Prepare("insert into account(AccountName,Password)values(?,?)")
			if err_stmt != nil {
				fmt.Println("err_stmt : ", err_stmt.Error())
				return
			}
			_, err_Exec := stmt.Exec(msg.GetAccountName(), msg.GetPassword())
			if err_Exec != nil {
				fmt.Println("err_Exec : ", err_Exec.Error())
				return
			}
			defer stmt.Close()
			err_commit := tx.Commit()
			if err_commit != nil {
				fmt.Println("err_commit : ", err_commit.Error())
				return
			}
			retMsg.RetCode = pb.GAS2CRegisterRet_LOGIN_SUCCESS.Enum()
			break
		}
	case 1:
		{
			//账号存在，不能注册
			fmt.Println("账号存在，不能注册")
			retMsg.RetCode = pb.GAS2CRegisterRet_LOGIN_FAIL_ACCOUNTEXIST.Enum()
			break
		}
	case 4:
		{
			//账号存在，不能注册
			fmt.Println("账号存在，不能注册")
			retMsg.RetCode = pb.GAS2CRegisterRet_LOGIN_FAIL_ACCOUNTEXIST.Enum()
			break
		}
	case 2:
		{
			//获取数据库数据出错
			fmt.Println("获取数据库数据出错")
			retMsg.RetCode = pb.GAS2CRegisterRet_LOGIN_FAIL.Enum()
			break
		}
	default:
		break
	}
	sendData, err := proto.Marshal(retMsg)
	if err != nil {
		fmt.Println("marshaling error: ", err)
	}
	//fmt.Println("ProcessMsg_C2GASLogin Ret : ", sendData)

	ProcessRetMsg(conn, sendData, (int32)(pbMsgEnum.GAS2CRegisterRet))
}

/*
	心跳
*/
func ProcessMsg_C2GSHeartTick(conn *net.TCPConn) {
	retMsg := &pb.C2GSHeartTickRet{}
	sendData, err := proto.Marshal(retMsg)
	if err != nil {
		fmt.Println("Error : ", err)
	}
	ProcessRetMsg(conn, sendData, (int32)(pbMsgEnum.GAS2CRegisterRet))
}

/*
	请求账号角色信息
*/
func ProcessMsg_C2GSRequestRoleInfo(conn *net.TCPConn, msg *pb.C2GSRequestRoleInfo) {
	db := GetConnectDB()
	defer db.Close()
	list := Table1.QueryRoleInfoListByAccountName(db, msg.GetAccountName())
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
}

/*
	进入游戏
*/
func ProcessMsg_C2GSEnterGame(conn *net.TCPConn, msg *pb.C2GSEnterGame) {
	db := GetConnectDB()
	defer db.Close()
	list := Table1.QueryRoleInfoListByAccountName(db, msg.GetAccountName())
	hasRole := false
	for i := 0; i < len(list); i++ {
		if list[i].RoleType == msg.SelectedRoleInfo.RoleType {
			hasRole = true
			break
		}
	}
	retMsg := &pb.GS2CEnterGameRet{}
	retMsg.AccountName = proto.String(msg.GetAccountName())
	NewRoleError := hasRole && *msg.IsNewRole
	if NewRoleError {
		retMsg.RetCode = pb.GS2CEnterGameRet_ENTER_FAIL.Enum()
	} else {
		retMsg.RetCode = pb.GS2CEnterGameRet_ENTER_SUCCESS.Enum()
		if *msg.IsNewRole {
			//新建角色
			CreateNewRole(db, msg.GetAccountName(), msg.SelectedRoleInfo)
		}
	}
	sendData, err := proto.Marshal(retMsg)
	if err != nil {
		fmt.Println("marshaling error: ", err)
	}
	ProcessRetMsg(conn, sendData, (int32)(pbMsgEnum.GS2CEnterGameRet))
	//登入游戏时，发送相关消息
	SendMsgByEnterGame(msg.GetAccountName(), msg.SelectedRoleInfo)
}

/*
	创建新角色
*/
func CreateNewRole(db *sql.DB, accountName string, info *pb.RoleInfo) {
	fmt.Println("创建新角色")
	tx, err_tx := db.Begin() //事务
	if err_tx != nil {
		fmt.Println("err_tx : ", err_tx.Error())
		return
	}
	switch *info.RoleType {
	case 1:
		{
			fmt.Println("角色1")
			stmt, err_stmt := tx.Prepare("insert into roletable(RoleOID,RoleType,RoleName,RoleLev)values(?,?,?,?)")
			if err_stmt != nil {
				fmt.Println("err_stmt : ", err_stmt.Error())
				return
			}
			_, err_Exec := stmt.Exec(accountName, info.RoleType, info.RoleName, 1)
			if err_Exec != nil {
				fmt.Println("err_Exec : ", err_Exec.Error())
				return
			}
			defer stmt.Close()
			err_commit := tx.Commit()
			if err_commit != nil {
				fmt.Println("err_commit : ", err_commit.Error())
				return
			}
		}
		break
	case 2:
		{
			fmt.Println("角色2")
			stmt, err_stmt := tx.Prepare("insert into account(AccountName,RoleType2,RoleName2,RoleLev2)values(?,?,?,?)")
			if err_stmt != nil {
				fmt.Println("err_stmt : ", err_stmt.Error())
				return
			}
			_, err_Exec := stmt.Exec(accountName, info.RoleType, info.RoleName, 1)
			if err_Exec != nil {
				fmt.Println("err_Exec : ", err_Exec.Error())
				return
			}
			defer stmt.Close()
			err_commit := tx.Commit()
			if err_commit != nil {
				fmt.Println("err_commit : ", err_commit.Error())
				return
			}
		}
		break
	case 3:
		{
			fmt.Println("角色3")
			stmt, err_stmt := tx.Prepare("insert into account(AccountName,RoleType3,RoleName3,RoleLev3)values(?,?,?,?)")
			if err_stmt != nil {
				fmt.Println("err_stmt : ", err_stmt.Error())
				return
			}
			_, err_Exec := stmt.Exec(accountName, info.RoleType, info.RoleName, 1)
			if err_Exec != nil {
				fmt.Println("err_Exec : ", err_Exec.Error())
				return
			}
			defer stmt.Close()
			err_commit := tx.Commit()
			if err_commit != nil {
				fmt.Println("err_commit : ", err_commit.Error())
				return
			}
		}
		break
	default:
		break
	}
}

/* 数据库数据添加
func Insert(tabelname string, params []string) {
	if Read(tabelname, params) {
		//表里有数据，更新
	} else {
		//表里无数据，插入
		paramStr := DecParams(params)
		s := []string{"insert into ", tabelname, "(", paramStr, ")values(?,?)"}
		strings.Join(s, "")
		stmt, err := test.db.Prepare(s)
		if err != nil {
			fmt.Println(err.Error())
			return
		}
		defer stmt.Close()
		if result, err := stmt.Exec("张三", 20); err == nil {
			if id, err := result.LastInsertId(); err == nil {
				fmt.Println("insert id : ", id)
			}
		}
		if result, err := stmt.Exec("李四", 30); err == nil {
			if id, err := result.LastInsertId(); err == nil {
				fmt.Println("insert id : ", id)
			}
		}
		if result, err := stmt.Exec("王五", 25); err == nil {
			if id, err := result.LastInsertId(); err == nil {
				fmt.Println("insert id : ", id)
			}
		}
	}
}*/

/* 测试数据库数据更新
func (test *TestMysql) Update() {
	if test.db == nil {
		return
	}
	stmt, err := test.db.Prepare("update table1 set name=?,age=? where age=?")
	if err != nil {
		fmt.Println(err.Error())
		return
	}
	defer stmt.Close()
	if result, err := stmt.Exec("周七", 40, 25); err == nil {
		if c, err := result.RowsAffected(); err == nil {
			fmt.Println("update count : ", c)
		}
	}
}*/

/* 数据库数据读取
func Read(tabelname string, params []string) bool {
	if test.db == nil {
		return false
	}
	paramStr := DecParams(params)
	s := []string{"select ", paramStr, " from ", tabelname}
	strings.Join(s, "")
	fmt.Println("Read s : ", s)
	rows, err := test.db.Query(s)
	if err != nil {
		fmt.Println(err.Error())
		return false
	}
	defer rows.Close()
	cols, _ := rows.Columns()
	for i := range cols {
		fmt.Print(cols[i])
		fmt.Print("\t")
	}
	fmt.Println("")
	var id int
	var name string
	var age int
	for rows.Next() {
		if err := rows.Scan(&id, &name, &age); err == nil {
			fmt.Print(id)
			fmt.Print("\t")
			fmt.Print(name)
			fmt.Print("\t")
			fmt.Print(age)
			fmt.Print("\t\r\n")
		}
	}
	return true
}*/

/* 数据库删除
func (test *TestMysql) Delete() {
	if test.db == nil {
		return
	}
	stmt, err := test.db.Prepare("delete from table1 where age=?")
	if err != nil {
		fmt.Println(err.Error())
		return
	}
	defer stmt.Close()
	if result, err := stmt.Exec(20); err == nil {
		if c, err := result.RowsAffected(); err == nil {
			fmt.Println("remove count : ", c)
		}
	}
}

func (test *TestMysql) Close() {
	if test.db != nil {
		test.db.Close()
	}
}

func SaveData() {

}

func main() {
	if test, err := Init(); err == nil {
		test.Create()
		test.Update()
		test.Read()
		test.Delete()
		test.Read()
		test.Close()
	}
}*/
