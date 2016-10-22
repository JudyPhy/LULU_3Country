using UnityEngine;
using System.Collections;


namespace sj {
	public class Events{

		public class Network {
			public const string ConnectDisconnected = "Network.ConnectDisconnected";
            public const string ConnectSuccessed_GateServer = "Network.ConnectSuccessed_GateServer";
			public const string ConnectFailed = "Network.ConnectFailed";
            public const string ConnectDisconnected_GateServer = "Network.ConnectDisconnected_GateServer";
		}


		public class AuthAccount {
            //public const string AuthSuccess = "AuthAccount.AuthSuccess";
			public const string AuthFailed = "AuthAccount.AuthFailed";
			public const string LoginGameSuccess = "AuthAccount.LoginGameSuccess";
			public const string LoginGameFailed = "AuthAccount.LoginGameFailed";
            public const string RegisterSuccess = "AuthAccount.RegisterSuccess";
            public const string LoginGASuccess = "AuthAccount.LoginGASuccess";
            public const string GateServerGet = "AuthAccount.GateServerGet";
            public const string ServerListGet = "AuthAccount.ServerListGet";
            public const string ServerSelect = "AuthAccount.ServerSelect";
		}

        // PVE: 副本 | 闯荡
        public class Pve {
            public const string ChapterOpen = "Pve.ChapterOpen";                               // 章节开启
            public const string ChapterFinish = "Pve.ChapterFinished";                         // 章节完成
            public const string ChapterItemClick = "Pve.ChapterItemClick";                     // 章节按钮点击通知
            public const string StageOpen = "Pve.StageOpen";                                   // 关卡开启
            public const string StageUpdate = "Pve.StageUpdate";                               // 关卡更新
            public const string StageFinish = "Pve.StageFinish";                               // 关卡完成
            public const string StageLevOpen = "Pve.StageLevOpen";                             // 副本开启
            public const string StageLevUpdate = "Pve.StageLevUpdate";                         // 副本更新
            public const string StageLevFinish = "Pve.StageLevFinish";                         // 副本完成
            public const string StageLevSelect = "Pve.StageLevSelect";                         // 副本被选中

            public const string EncounterModeUpdate = "Pve.EncounterModeUpdate";               // 闯荡模式更新
            public const string EncounterWorkStateUpdate = "Pve.EncounterWorkStateUpdate";     // 闯荡状态有变化
            public const string EncounterEventsUpdate = "Pve.EncounterEventsUpdate";           // 闯荡遭遇更新

			// 战斗中
            public const string ShowBattleReward = "Pve.ShowBattleReward";                     // 显示奖励
            public const string QuickPveResetCount = "Pve.QuickPveResetCount";                 // 更新挑战次数
            public const string ChanllengeCount = "Pve.ChanllengeCount";                       // 日常挑战次数

            public const string SelectChapter = "Pve.SelectChapter";    //点击某个章节
            public const string SelectStage = "Pve.SelectStage";    //点击某个章节关卡
            public const string EncounterRet = "EncounterRet";              //挂机返回
            public const string EncounterRewardRet = "EncounterRewardRet";    //挂机奖励返回
            public const string EncounterTimeUp = "EncounterUp";              //状态更新
            public const string UpdatePveStageLevSelectStatus = "Pve.UpdatePveStageLevSelectStatus";    //点击某个副本
            public const string ClickChallenge = "ClickChallenge";
            public const string UpdateChallengeAndResetCountUI = "Pve.UpdateChallengeAndResetCountUI";   //更新挑战和重置次数
        }

        // 角色: 玩家 | NPC | 侠客
        public class Role {
            public const string PlayerLevUpdateTips = "Role.PlayerLevUpdate";
            public const string UpdateUIByCompose = "Role.UpdateUIByCompose";                                     // 合成后，侠客列表刷新

            public const string ChangeIconUI = "Role.ChangeIconUI";                             // 更新头像列表选择状态 
            public const string ChangeIconInfo = "Role.ChangeIconInfo";                         // 更新主界面头像
			public const string CreateRole = "Role.CreateRole";	                                // 新玩家创建角色
            public const string PlayerProUpdate = "PlayerProUpdate";                                  // 玩家信息更新

        }

        // 物品: 装备 | 秘籍 | 书籍 | 道具
        public class Item {
            public const string ItemRemove = "Item.ItemRemove";      
            public const string SellRet = "Item.SellRet"; //出售后，刷新列表
            public const string SetStoreShown = "Item.SetStoreShown";
        }

        // 背包/行囊
        public class Package {
            public const string PackageItemUpdate = "Package.ItemUpdate";       //道具列表变化时，刷新背包列表                
            public const string EncounterUpdate = "Encounter.Update";
            public const string EncounterItemUpdate = "Encounter.ItemUpdate";
            public const string SelecteSellItem = "Package.SelecteSellItem";       //选择出售物品时，更新总价格
            public const string ItemSellAllSelect = "Item.ItemSellSelect";  //点击全选按钮时，刷新UI
        }

        // 提示: Tips | TipsState
        public class Tips {
            public const string TipsStateUpdate = "Tips.TipsStateUpdate";
        }

		//剧情对话逻辑
		public class StoryLogic {
			public const string ShowBattleDialogueInBattle = "xBattlePanel.ShowBattleDialogueInBattle";
			public const string StartStoryLogicByStageLevID = "StoryLogicObject.StartStoryLogicByStageLevID";
			public const string StartStoryLogicByStoryID = "StoryLogicObject.StartStoryLogicByStoryID";
            public const string StartRandomStory = "StoryLogicObject.StartRandomStory";
		}

        //随机商店
        public class RandomShop {
            public const string UpdateRandomShopUI = "RandomShopPanel.UpdateRandomShopUI";
            public const string ShowRandomShop = "HomeUI.ShowRandomShop";
            public const string CloseHomeRandomShop = "HomeUI.CloseHomeRandomShop";
        }

        //普通商店
        public class Commodity {
            public const string UpdateCommodityUI = "CommodityPanel.UpdateCommodityUI";
        }

        //竞技场
        public class PVP{
            public const string UpdatePVPStoreUI = "PvpChallengeListPanel.UpdatePVPStoreUI";    //购买后刷新竞技场商店购买次数
            public const string UpdateStoreTopPrestigeUI = "PvpChallengeListPanel.UpdateStoreTopPrestigeUI";    //威望变化时，刷新竞技场商店UI
            public const string UpdatePVPAvailbleState = "PvpChallengeListPanel.UpdatePVPAvailbleState";        //刷新pvp红点
            public const string UpdateChallengeList = "PvpChallengeListPanel.UpdateChallengeList";
            public const string UpdatePvpExpeditionPanel = "PvpExpeditionPanel";                                //刷新远征UP
        }

        // 阵容
        public class Formation {
            public const string DragHeroBtnOver = "FormationPanel.DragHeroBtnOver"; //拖曳头像结束
            public const string UpdateFormtationRet = "FormationPanel.UpdateFormtationRet"; //更新侠客阵容反馈
            public const string DescMoveToTarget = "FormationPanel.DescMoveToTarget"; //点击有侠客的herobtn（面板滑到指定位置）
            public const string ReshowHeroBtnByFormationOverTime = "FormationPanel.ReshowHeroBtnByFormationOverTime"; //拖曳头像请求改变阵型，超时后恢复原位置
            public const string UpdateLevUI = "FormationPanel.UpdateLevUI"; //英雄等级变化时，界面刷新
            public const string UpdateExpSliderUI = "FormationPanel.UpdateExpSliderUI"; //英雄经验变化时，界面刷新
            public const string GrowPotentialOrQuaRet = "FormationPanel.GrowPotentialRet"; //资质变化时，界面刷新
            public const string LoadOrUnloadEquip = "FormationPanel.LoadOrUnloadEquip"; //装备穿戴情况变化时，界面刷新
            public const string LoadOrUnloadEsoterica = "FormationPanel.LoadOrUnloadEsoterica"; //秘籍穿戴情况变化时，界面刷新
            public const string LoadOrUnloadEquipForBattle = "FormationPanel.LoadOrUnloadEquipForBattle"; //战斗力变化时
            public const string GrowEsotericaSkillRet = "FormationPanel.GrowEsotericaSkillRet"; //秘籍研习反馈
        }


        public class RoleGrow {
            public const string LevUpTips = "RoleGrow.LevUpTips";  
            public const string GrowPotentialOrQuaSuccessEffect = "RoleGrow.GrowPotentialOrQuaSuccessEffect";  // 修炼/领悟 成功特效
            public const string ChangeHero = "RoleGrow.ChangeHero";                     // 更换侠客

            public const string UpdateGrowExp = "RoleGrow.UpdateGrowExp";                     // 更新经验条

            public const string UpdateLevUI = "RoleGrow.UpdateLevUI";         //等级提升
            public const string UpdateExpSliderUI = "RoleGrow.UpdateExpSliderUI";         //经验增加
            public const string UseExpItemRet = "RoleGrow.UseExpItemRet";         //使用历练丹后，经验和等级刷新
        }

        public class HomeUI {
            public const string UpdatePlayerInfo = "HomeUI.UpdatePlayerInfo";                       // 主角属性变化时，通知所有显示属性的界面刷新ui
            public const string UpdateBtnState = "HomeUI.UpdateBtnState";                           // 主界面按钮状态刷新

            public const string UpdateArrowState = "HomeUI.UpdateArrowState";       //主界面箭头红点更新
            public const string UpdatePlayerExpUI = "HomeUI.UpdatePlayerExpUI";
            public const string UpdatePlayerLevUI = "HomeUI.UpdatePlayerLevUI";
            public const string UpdatePlayerMoneyUI = "HomeUI.UpdatePlayerMoneyUI";
            public const string UpdatePlayerGemUI = "HomeUI.UpdatePlayerGemUI";
            public const string UpdatePlayerSolidFoodUI = "HomeUI.UpdatePlayerSolidFoodUI";
            public const string UpdatePlayerSkillExpUI = "HomeUI.UpdatePlayerSkillExpUI";
        }

        // 任务
        public class Task {
            public const string UpdateUIByUpdateTaskData = "Task.UpdateUIByUpdateTaskData";                             // 刷新TaskPanel
            public const string OpenDailyTask = "TaskPanel.OpenDailyTask";
            public const string UpdateUIByUpdateScoreNode = "Task.UpdateUIByUpdateScoreNode";                             // 刷新TaskPanel
        }

        // 签到
        public class Sign {
            public const string SignRet = "Sign.SignRet";                       // 签到反馈
        }

        public class EquipEsoterica {
            public const string UpdateEquiptEsotericaLev = "EquipEsoterica.UpdateEquiptLev";         //参数（1天赋 2秘籍）

            public const string ShowDetailInfo = "EquipEsoterica.ShowDetailInfo";   //点击箭头，查看装备详细信息
            public const string GrowLevRet = "EquipEsoterica.GrowLevRet";       //装备强化反馈
            public const string SelectGrowQuaEquipment = "EquipEsoterica.SelectGrowQuaEquipment";       //选择装备进阶消耗物品
            public const string GrowQuaRet = "EquipEsoterica.GrowQuaRet";       //装备进阶反馈
            public const string XilianRet = "EquipEsoterica.XilianRet";       //装备洗练反馈
            public const string XilianRestoreRet = "EquipEsoterica.XilianRestoreRet";       //装备洗练恢复反馈
            public const string UpdateTanlentSkillLev = "EquipEsoterica.UpdateTanlentSkillLev";    //天赋研习反馈
            public const string GrowSkillLevRetForBtn = "EquipEsoterica.GrowSkillLevRetForBtn";    //技能研习反馈，修改按钮状态
            public const string UpdateListUIByAddOrRemoveItems = "EquipEsoterica.UpdateListUIByAddOrRemoveItems";    //添加或移除道具时，刷新列表
        }

        // 聊天
        public class Chat {
            public const string UpdateCurChannel = "Chat.UpdateCurChannel";             // 聊天当前窗口更新
            //public const string UpdateHorn = "Chat.UpdateHorn";                         // 聊天小喇叭更新      
            public const string ClickName = "Chat.ClickName";                           // 聊天消息有名字被点击
            public const string OpenPrivateChannel = "Chat.OpenPrivateChannel";         // 聊天打开私聊UI
        }

        // 创建
        public class CreateRole {
            public const string Error = "CreateRole.Error";                                 // 创建角色时的错误提示
            public const string LoginRet = "CreateRole.LoginRet";                                 // 登录游戏服务器反馈
        }

        // 探索
        public class Explore {
            public const string UpdateMissionUI = "Explore.UpdateMissionUI";            // 刷新MissionUnExplorePanel
            public const string SkillUpdate = "Explore.UpdateMissionUI";                // 探索技能刷新
            public const string ClickChapterId = "ClickChapterId";                      // 点击顶部关卡

            public const string SelectHero = "Explore.SelectHero";                      //选择派遣侠客
            public const string UpdateUI = "Explore.UpdateUI";                          // 刷新ExplorePanel
            public const string CurMissionData = "CurMissionData";                      // 当前最近任务
        }

        // 好友
        public class Friend {
            public const string ListUpdate = "Friend.ListUpdate";                       // 好友列表刷新
        }

        // 邮件
        public class Mail {
            public const string UpdateUIByChangeMailList = "Mail.UpdateUIByChangeMailList";       //列表数据改变时，刷新UI
            public const string ReceiveAppendixResult = "Mail.ReceiveAppendixResult";   // 领取附件反馈
            public const string StatusUpdate = "Mail.StatusUpdate";                     // 邮件阅读状态刷新
        }

        // 剧情
        public class Story {
            public const string Next = "Story.Next";                                    // 剧情，显示下一段对话
            public const string ClickOption = "Story.ClickOption";                      // 剧情，点击选项
            public const string WaitPlayerSelect = "Story.WaitPlayerSelect";            // 剧情，等待玩家选择
            public const string NextBottom = "Story.NextBottom";                        // 剧情下一条底部
            public const string NextTop = "Story.NextTop";                              // 剧情下一条顶部
        }

        public class ContinusLogin {
            public const string UpdateContinusLoginList = "ContinusLoginPanel.UpdateContinusLoginList";
            public const string ShowItem = "ContinusLoginPanel.ShowItem";
        }

        public class DrawTreasure {
            public const string ShowRewardList = "DrawTreasurePanel.ShowRewardList";
            public const string UpdateMoney = "DrawTreasurePanel.UpdateMoney";
            public const string UpdateTimes = "DrawTreasurePanel.UpdateTimes";
            public const string UpdateMemberCardUIByGetReward = "DrawTreasurePanel.UpdateMemberCardUIByGetReward";  //领取月卡奖励反馈
        }
	}
}