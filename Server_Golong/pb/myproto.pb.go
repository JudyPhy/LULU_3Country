// Code generated by protoc-gen-go.
// source: myproto.proto
// DO NOT EDIT!

/*
Package pb is a generated protocol buffer package.

It is generated from these files:
	myproto.proto

It has these top-level messages:
	C2GSHeartTick
	C2GSHeartTickRet
	C2GASLogin
	GAS2CLoginRet
	C2GASRegister
	GAS2CRegisterRet
	C2GSRequestRoleInfo
	RoleInfo
	GS2CRequestRoleInfoRet
	C2GSEnterGame
	GS2CEnterGameRet
	GS2CRoleDetailInfo
	EquipmentData
*/
package pb

import proto "code.google.com/p/goprotobuf/proto"
import math "math"

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = math.Inf

type GAS2CLoginRet_ErrorCode int32

const (
	GAS2CLoginRet_LOGIN_SUCCESS         GAS2CLoginRet_ErrorCode = 1
	GAS2CLoginRet_LOGIN_FAIL            GAS2CLoginRet_ErrorCode = 2
	GAS2CLoginRet_LOGIN_ACCOUNTNAME_ERR GAS2CLoginRet_ErrorCode = 3
	GAS2CLoginRet_LOGIN_PWD_ERR         GAS2CLoginRet_ErrorCode = 4
)

var GAS2CLoginRet_ErrorCode_name = map[int32]string{
	1: "LOGIN_SUCCESS",
	2: "LOGIN_FAIL",
	3: "LOGIN_ACCOUNTNAME_ERR",
	4: "LOGIN_PWD_ERR",
}
var GAS2CLoginRet_ErrorCode_value = map[string]int32{
	"LOGIN_SUCCESS":         1,
	"LOGIN_FAIL":            2,
	"LOGIN_ACCOUNTNAME_ERR": 3,
	"LOGIN_PWD_ERR":         4,
}

func (x GAS2CLoginRet_ErrorCode) Enum() *GAS2CLoginRet_ErrorCode {
	p := new(GAS2CLoginRet_ErrorCode)
	*p = x
	return p
}
func (x GAS2CLoginRet_ErrorCode) String() string {
	return proto.EnumName(GAS2CLoginRet_ErrorCode_name, int32(x))
}
func (x *GAS2CLoginRet_ErrorCode) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(GAS2CLoginRet_ErrorCode_value, data, "GAS2CLoginRet_ErrorCode")
	if err != nil {
		return err
	}
	*x = GAS2CLoginRet_ErrorCode(value)
	return nil
}

type GAS2CRegisterRet_ErrorCode int32

const (
	GAS2CRegisterRet_LOGIN_SUCCESS           GAS2CRegisterRet_ErrorCode = 1
	GAS2CRegisterRet_LOGIN_FAIL_ACCOUNTEXIST GAS2CRegisterRet_ErrorCode = 2
	GAS2CRegisterRet_LOGIN_FAIL              GAS2CRegisterRet_ErrorCode = 3
)

var GAS2CRegisterRet_ErrorCode_name = map[int32]string{
	1: "LOGIN_SUCCESS",
	2: "LOGIN_FAIL_ACCOUNTEXIST",
	3: "LOGIN_FAIL",
}
var GAS2CRegisterRet_ErrorCode_value = map[string]int32{
	"LOGIN_SUCCESS":           1,
	"LOGIN_FAIL_ACCOUNTEXIST": 2,
	"LOGIN_FAIL":              3,
}

func (x GAS2CRegisterRet_ErrorCode) Enum() *GAS2CRegisterRet_ErrorCode {
	p := new(GAS2CRegisterRet_ErrorCode)
	*p = x
	return p
}
func (x GAS2CRegisterRet_ErrorCode) String() string {
	return proto.EnumName(GAS2CRegisterRet_ErrorCode_name, int32(x))
}
func (x *GAS2CRegisterRet_ErrorCode) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(GAS2CRegisterRet_ErrorCode_value, data, "GAS2CRegisterRet_ErrorCode")
	if err != nil {
		return err
	}
	*x = GAS2CRegisterRet_ErrorCode(value)
	return nil
}

type GS2CEnterGameRet_ErrorCode int32

const (
	GS2CEnterGameRet_ENTER_SUCCESS GS2CEnterGameRet_ErrorCode = 1
	GS2CEnterGameRet_ENTER_FAIL    GS2CEnterGameRet_ErrorCode = 2
)

var GS2CEnterGameRet_ErrorCode_name = map[int32]string{
	1: "ENTER_SUCCESS",
	2: "ENTER_FAIL",
}
var GS2CEnterGameRet_ErrorCode_value = map[string]int32{
	"ENTER_SUCCESS": 1,
	"ENTER_FAIL":    2,
}

func (x GS2CEnterGameRet_ErrorCode) Enum() *GS2CEnterGameRet_ErrorCode {
	p := new(GS2CEnterGameRet_ErrorCode)
	*p = x
	return p
}
func (x GS2CEnterGameRet_ErrorCode) String() string {
	return proto.EnumName(GS2CEnterGameRet_ErrorCode_name, int32(x))
}
func (x *GS2CEnterGameRet_ErrorCode) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(GS2CEnterGameRet_ErrorCode_value, data, "GS2CEnterGameRet_ErrorCode")
	if err != nil {
		return err
	}
	*x = GS2CEnterGameRet_ErrorCode(value)
	return nil
}

// 心跳 C->GS
type C2GSHeartTick struct {
	XXX_unrecognized []byte `json:"-"`
}

func (m *C2GSHeartTick) Reset()         { *m = C2GSHeartTick{} }
func (m *C2GSHeartTick) String() string { return proto.CompactTextString(m) }
func (*C2GSHeartTick) ProtoMessage()    {}

// 心跳 GS->C
type C2GSHeartTickRet struct {
	XXX_unrecognized []byte `json:"-"`
}

func (m *C2GSHeartTickRet) Reset()         { *m = C2GSHeartTickRet{} }
func (m *C2GSHeartTickRet) String() string { return proto.CompactTextString(m) }
func (*C2GSHeartTickRet) ProtoMessage()    {}

// 登录网关服务器
type C2GASLogin struct {
	AccountName      *string `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	Password         *string `protobuf:"bytes,2,opt" json:"Password,omitempty"`
	XXX_unrecognized []byte  `json:"-"`
}

func (m *C2GASLogin) Reset()         { *m = C2GASLogin{} }
func (m *C2GASLogin) String() string { return proto.CompactTextString(m) }
func (*C2GASLogin) ProtoMessage()    {}

func (m *C2GASLogin) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *C2GASLogin) GetPassword() string {
	if m != nil && m.Password != nil {
		return *m.Password
	}
	return ""
}

// 登录网关服务器回馈
type GAS2CLoginRet struct {
	RetCode          *GAS2CLoginRet_ErrorCode `protobuf:"varint,1,req,enum=pb.GAS2CLoginRet_ErrorCode" json:"RetCode,omitempty"`
	AccountName      *string                  `protobuf:"bytes,2,opt" json:"AccountName,omitempty"`
	HasRole          *bool                    `protobuf:"varint,3,opt" json:"HasRole,omitempty"`
	XXX_unrecognized []byte                   `json:"-"`
}

func (m *GAS2CLoginRet) Reset()         { *m = GAS2CLoginRet{} }
func (m *GAS2CLoginRet) String() string { return proto.CompactTextString(m) }
func (*GAS2CLoginRet) ProtoMessage()    {}

func (m *GAS2CLoginRet) GetRetCode() GAS2CLoginRet_ErrorCode {
	if m != nil && m.RetCode != nil {
		return *m.RetCode
	}
	return GAS2CLoginRet_LOGIN_SUCCESS
}

func (m *GAS2CLoginRet) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *GAS2CLoginRet) GetHasRole() bool {
	if m != nil && m.HasRole != nil {
		return *m.HasRole
	}
	return false
}

// 注册账号
type C2GASRegister struct {
	AccountName      *string `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	Password         *string `protobuf:"bytes,2,opt" json:"Password,omitempty"`
	XXX_unrecognized []byte  `json:"-"`
}

func (m *C2GASRegister) Reset()         { *m = C2GASRegister{} }
func (m *C2GASRegister) String() string { return proto.CompactTextString(m) }
func (*C2GASRegister) ProtoMessage()    {}

func (m *C2GASRegister) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *C2GASRegister) GetPassword() string {
	if m != nil && m.Password != nil {
		return *m.Password
	}
	return ""
}

// 注册账号回馈
type GAS2CRegisterRet struct {
	RetCode          *GAS2CRegisterRet_ErrorCode `protobuf:"varint,1,req,enum=pb.GAS2CRegisterRet_ErrorCode" json:"RetCode,omitempty"`
	AccountName      *string                     `protobuf:"bytes,2,opt" json:"AccountName,omitempty"`
	XXX_unrecognized []byte                      `json:"-"`
}

func (m *GAS2CRegisterRet) Reset()         { *m = GAS2CRegisterRet{} }
func (m *GAS2CRegisterRet) String() string { return proto.CompactTextString(m) }
func (*GAS2CRegisterRet) ProtoMessage()    {}

func (m *GAS2CRegisterRet) GetRetCode() GAS2CRegisterRet_ErrorCode {
	if m != nil && m.RetCode != nil {
		return *m.RetCode
	}
	return GAS2CRegisterRet_LOGIN_SUCCESS
}

func (m *GAS2CRegisterRet) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

// 请求角色信息
type C2GSRequestRoleInfo struct {
	AccountName      *string `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	XXX_unrecognized []byte  `json:"-"`
}

func (m *C2GSRequestRoleInfo) Reset()         { *m = C2GSRequestRoleInfo{} }
func (m *C2GSRequestRoleInfo) String() string { return proto.CompactTextString(m) }
func (*C2GSRequestRoleInfo) ProtoMessage()    {}

func (m *C2GSRequestRoleInfo) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

// 角色信息数据
type RoleInfo struct {
	RoleType         *int32  `protobuf:"varint,1,opt" json:"RoleType,omitempty"`
	RoleLev          *int32  `protobuf:"varint,2,opt" json:"RoleLev,omitempty"`
	RoleName         *string `protobuf:"bytes,3,opt" json:"RoleName,omitempty"`
	XXX_unrecognized []byte  `json:"-"`
}

func (m *RoleInfo) Reset()         { *m = RoleInfo{} }
func (m *RoleInfo) String() string { return proto.CompactTextString(m) }
func (*RoleInfo) ProtoMessage()    {}

func (m *RoleInfo) GetRoleType() int32 {
	if m != nil && m.RoleType != nil {
		return *m.RoleType
	}
	return 0
}

func (m *RoleInfo) GetRoleLev() int32 {
	if m != nil && m.RoleLev != nil {
		return *m.RoleLev
	}
	return 0
}

func (m *RoleInfo) GetRoleName() string {
	if m != nil && m.RoleName != nil {
		return *m.RoleName
	}
	return ""
}

// 请求角色信息反馈
type GS2CRequestRoleInfoRet struct {
	AccountName      *string     `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	RoleInfoList     []*RoleInfo `protobuf:"bytes,2,rep" json:"RoleInfoList,omitempty"`
	XXX_unrecognized []byte      `json:"-"`
}

func (m *GS2CRequestRoleInfoRet) Reset()         { *m = GS2CRequestRoleInfoRet{} }
func (m *GS2CRequestRoleInfoRet) String() string { return proto.CompactTextString(m) }
func (*GS2CRequestRoleInfoRet) ProtoMessage()    {}

func (m *GS2CRequestRoleInfoRet) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *GS2CRequestRoleInfoRet) GetRoleInfoList() []*RoleInfo {
	if m != nil {
		return m.RoleInfoList
	}
	return nil
}

// 登录角色
type C2GSEnterGame struct {
	AccountName      *string   `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	SelectedRoleInfo *RoleInfo `protobuf:"bytes,2,opt" json:"SelectedRoleInfo,omitempty"`
	IsNewRole        *bool     `protobuf:"varint,3,opt" json:"IsNewRole,omitempty"`
	XXX_unrecognized []byte    `json:"-"`
}

func (m *C2GSEnterGame) Reset()         { *m = C2GSEnterGame{} }
func (m *C2GSEnterGame) String() string { return proto.CompactTextString(m) }
func (*C2GSEnterGame) ProtoMessage()    {}

func (m *C2GSEnterGame) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *C2GSEnterGame) GetSelectedRoleInfo() *RoleInfo {
	if m != nil {
		return m.SelectedRoleInfo
	}
	return nil
}

func (m *C2GSEnterGame) GetIsNewRole() bool {
	if m != nil && m.IsNewRole != nil {
		return *m.IsNewRole
	}
	return false
}

// 登录角色反馈
type GS2CEnterGameRet struct {
	AccountName      *string                     `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	RetCode          *GS2CEnterGameRet_ErrorCode `protobuf:"varint,2,req,enum=pb.GS2CEnterGameRet_ErrorCode" json:"RetCode,omitempty"`
	XXX_unrecognized []byte                      `json:"-"`
}

func (m *GS2CEnterGameRet) Reset()         { *m = GS2CEnterGameRet{} }
func (m *GS2CEnterGameRet) String() string { return proto.CompactTextString(m) }
func (*GS2CEnterGameRet) ProtoMessage()    {}

func (m *GS2CEnterGameRet) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *GS2CEnterGameRet) GetRetCode() GS2CEnterGameRet_ErrorCode {
	if m != nil && m.RetCode != nil {
		return *m.RetCode
	}
	return GS2CEnterGameRet_ENTER_SUCCESS
}

// 角色详细信息
type GS2CRoleDetailInfo struct {
	AccountName       *string          `protobuf:"bytes,1,opt" json:"AccountName,omitempty"`
	SelectedRoleInfo  *RoleInfo        `protobuf:"bytes,2,opt" json:"SelectedRoleInfo,omitempty"`
	Exp               *int32           `protobuf:"varint,3,opt" json:"Exp,omitempty"`
	Money             *int32           `protobuf:"varint,4,opt" json:"Money,omitempty"`
	Diamond           *int32           `protobuf:"varint,5,opt" json:"Diamond,omitempty"`
	VIP               *int32           `protobuf:"varint,6,opt" json:"VIP,omitempty"`
	EquipmentDataList []*EquipmentData `protobuf:"bytes,7,rep" json:"EquipmentDataList,omitempty"`
	XXX_unrecognized  []byte           `json:"-"`
}

func (m *GS2CRoleDetailInfo) Reset()         { *m = GS2CRoleDetailInfo{} }
func (m *GS2CRoleDetailInfo) String() string { return proto.CompactTextString(m) }
func (*GS2CRoleDetailInfo) ProtoMessage()    {}

func (m *GS2CRoleDetailInfo) GetAccountName() string {
	if m != nil && m.AccountName != nil {
		return *m.AccountName
	}
	return ""
}

func (m *GS2CRoleDetailInfo) GetSelectedRoleInfo() *RoleInfo {
	if m != nil {
		return m.SelectedRoleInfo
	}
	return nil
}

func (m *GS2CRoleDetailInfo) GetExp() int32 {
	if m != nil && m.Exp != nil {
		return *m.Exp
	}
	return 0
}

func (m *GS2CRoleDetailInfo) GetMoney() int32 {
	if m != nil && m.Money != nil {
		return *m.Money
	}
	return 0
}

func (m *GS2CRoleDetailInfo) GetDiamond() int32 {
	if m != nil && m.Diamond != nil {
		return *m.Diamond
	}
	return 0
}

func (m *GS2CRoleDetailInfo) GetVIP() int32 {
	if m != nil && m.VIP != nil {
		return *m.VIP
	}
	return 0
}

func (m *GS2CRoleDetailInfo) GetEquipmentDataList() []*EquipmentData {
	if m != nil {
		return m.EquipmentDataList
	}
	return nil
}

// 装备数据结构
type EquipmentData struct {
	OID              *int32 `protobuf:"varint,1,opt" json:"OID,omitempty"`
	ConfigID         *int32 `protobuf:"varint,2,opt" json:"ConfigID,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *EquipmentData) Reset()         { *m = EquipmentData{} }
func (m *EquipmentData) String() string { return proto.CompactTextString(m) }
func (*EquipmentData) ProtoMessage()    {}

func (m *EquipmentData) GetOID() int32 {
	if m != nil && m.OID != nil {
		return *m.OID
	}
	return 0
}

func (m *EquipmentData) GetConfigID() int32 {
	if m != nil && m.ConfigID != nil {
		return *m.ConfigID
	}
	return 0
}

func init() {
	proto.RegisterEnum("pb.GAS2CLoginRet_ErrorCode", GAS2CLoginRet_ErrorCode_name, GAS2CLoginRet_ErrorCode_value)
	proto.RegisterEnum("pb.GAS2CRegisterRet_ErrorCode", GAS2CRegisterRet_ErrorCode_name, GAS2CRegisterRet_ErrorCode_value)
	proto.RegisterEnum("pb.GS2CEnterGameRet_ErrorCode", GS2CEnterGameRet_ErrorCode_name, GS2CEnterGameRet_ErrorCode_value)
}