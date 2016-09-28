// Table1
package Table1

import (
	"database/sql"
	"fmt"
	_ "github.com/go-sql-driver/mysql"
	"testClient/pb"
)

/*
	检查玩家账号密码（1.账号密码正确 0.账号不存在 2.获取数据库数据出错 4.密码错误）
*/
func CheckAccountNameAndPassword(baseDb *sql.DB, primaryKeyValue string, password string) int {
	rows, err := baseDb.Query("select AccountName,Password from account")
	if err != nil {
		fmt.Println("Query AccountOID Error : ", err.Error())
		return 2
	}
	defer rows.Close()
	var accountNameIn string
	var passwordIn string
	for rows.Next() {
		if err := rows.Scan(&accountNameIn, &passwordIn); err == nil {
			if accountNameIn == primaryKeyValue {
				passwordRight := passwordIn == password
				if passwordRight {
					return 1
				} else {
					return 4
				}
			}
		}
	}
	return 0
}

/*
	获取玩家账号对应角色信息列表
*/
func QueryRoleInfoListByAccountName(baseDb *sql.DB, primaryKeyValue string) []pb.RoleInfo {
	rows, err := baseDb.Query("select AccountName,RoleOID1,RoleOID2,RoleOID3 from account")
	if err != nil {
		fmt.Println("Query RoleOID Error : ", err.Error())
		return nil
	}
	defer rows.Close()
	var accountNameIn string
	roleOID := []string{"", "", ""}
	for rows.Next() {
		if err := rows.Scan(&accountNameIn, &roleOID[0], &roleOID[1], &roleOID[2]); err == nil {
			if accountNameIn == primaryKeyValue {
				break
			}
		}
	}
	fmt.Println("角色列表 : ", len(roleOID))
	result := []pb.RoleInfo{}
	index := 0
	var roleOid string
	var roleType int
	var roleName string
	var roleLev int
	for i := 0; i < len(roleOID); i++ {
		fmt.Println("角色OID : ", roleOID[i])
		if len(roleOID[i]) > 0 {
			rows, err = baseDb.Query("select RoleOID,RoleType,RoleName,RoleLev from roletable")
			if err != nil {
				fmt.Println("Query RoleTable Error : ", err.Error())
				return nil
			}
			for rows.Next() {
				if err := rows.Scan(&roleOid, &roleType, &roleName, &roleLev); err == nil {
					if roleOid == roleOID[i] {
						role := &pb.RoleInfo{}
						role.RoleName = &roleName
						role.RoleType = &roleType
						role.RoleLev = &roleLev
						result[index] = *role
						index++
						break
					}
				}
			}
		}
	}
	return result
}
