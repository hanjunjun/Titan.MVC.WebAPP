
using System.ComponentModel;

namespace Titan.Infrastructure.Domain
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class Constants
    {
        #region 老人状态

        /// <summary>
        /// 入住
        /// </summary>
        public const int CStateCheckIn = 2;

        /// <summary>
        /// 外出
        /// </summary>
        public const int CStateOut = 3;

        /// <summary>
        /// 退住待结算
        /// </summary>
        public const int CStateWaitStayBack = 4;

        /// <summary>
        /// 退住
        /// </summary>
        public const int CStateStayBack = 5;

        #endregion

        #region 杨浦老人状态

        /// <summary>
        /// 入住
        /// </summary>
        public const int YpCstateCheckIn = 4;

        /// <summary>
        /// 外出
        /// </summary>
        public const int YpCstateOut = 5;

        /// <summary>
        /// 退住待结算
        /// </summary>
        public const int YpCstateWaitStayBack = 6;

        /// <summary>
        /// 退住
        /// </summary>
        public const int YpCstateStayBack = 7;

        #endregion

        #region 员工类别

        /// <summary>
        /// 护工
        /// </summary>
        public const int NursingWorkers = 0;

        /// <summary>
        /// 护士
        /// </summary>
        public const int Nurse = 1;

        /// <summary>
        /// 医生
        /// </summary>
        public const int Doctor = 2;

        /// <summary>
        /// 社工
        /// </summary>
        public const int SocialWorker = 3;

        /// <summary>
        /// 财务人员
        /// </summary>
        public const int FinaicalWorker = 7;

        /// <summary>
        /// 管理员
        /// </summary>
        public const int Administrator = 10000;

        #endregion

        #region 弹出框名称

        public const string AddViewName = "新增";
        public const string EditViewName = "编辑";
        public const string LookViewName = "查看";

        #endregion

        #region 基础费用类型

        /// <summary>
        /// 床位费
        /// </summary>
        public const int BedFee = 0;

        /// <summary>
        /// 护理费
        /// </summary>
        public const int NursFee = 1;

        /// <summary>
        /// 膳食费
        /// </summary>
        public const int EatFee = 2;

        /// <summary>
        /// 服务费
        /// </summary>
        public const int ServeFee = 3;

        /// <summary>
        /// 其他月费
        /// </summary>
        public const int OtherMonthFee = 4;

        /// <summary>
        /// 有偿服务费
        /// </summary>
        public const int PaidServiceFee = 5;

        #endregion

        #region 初始费用类型

        /// <summary>
        /// 保险费类型。在几个初始费用中，保险费需特殊处理，保险费在退住时 不进行退费
        /// </summary>
        public const int PremiumFee = 4;
        /// <summary>
        /// 杂费不退款
        /// </summary>
        public const int Incidentals = 5;

        #endregion

        #region 特殊字典

        /// <summary>
        /// 护理项目类型，字典
        /// </summary>
        public const string NursingProjectTypeDataType = "NurProType";

        #endregion

        #region 卡类型

        public enum CardType
        {
            NoExist = -1,
            OldCard = 0,
            BuidCard = 1,
            RoomCard = 2
        }

        #endregion

        #region 变更类型

        public enum ElderLiveChangeType
        {
            BedChange = 0,//床位变更
            NursingLevelChange = 1,//护理级别变更
            EatingChange = 2,//膳食类型变更
            RoomChange = 3//包房类型变更,基础服务变更
        }

        #endregion

        #region 杨浦 变更状态

        public enum YpElderLiveChangeState
        {
            Delete = 0,//
            WaitSure = 1,//待审核
            WaitCheck = 2,//待审批
            WaitSignature = 3,//待家属确认
            Working = 4//已通过
        }

        #endregion

        #region App类型

        public enum AppType
        {
            [Description("移动APP护理端")]
            NursingApp = 0,
            [Description("移动APP管理端")]
            ManageApp = 1,
            [Description("移动APP家属端")]
            FamilyApp = 2
        }

        #endregion

        #region 包名
        public static class AppPackages
        {
            /// <summary>
            /// 护理
            /// </summary>
            public const string NursingApp = "com.Titan.Han .sknis";

            /// <summary>
            /// 管理
            /// </summary>
            public const string ManageApp = "com.Titan.Han .sknis.manager";

            /// <summary>
            /// 家属
            /// </summary>
            public const string FamilyApp = "com.Titan.Han .sknis.family";
        }
        #endregion

        #region 审批相关

        /// <summary>
        /// 审批类型
        /// </summary>
        public enum CheckType
        {
            YpElderLiveChange = 0
        }

        //审批结果
        public enum CheckResult
        {
            Disagree = 0,
            Agree = 1,
            NoResult = 2
        }

        #endregion

        #region 金蝶

        #region 收款组织/结算组织（常量）
        /// <summary>
        /// 现金
        /// </summary>
        public const string ORGID = "100";
        #endregion

        #region 结算方式（常量）
        /// <summary>
        /// 现金
        /// </summary>
        public const string JSFS01_SYS = "JSFS01_SYS";
        /// <summary>
        /// 现金支票
        /// </summary>
        public const string JSFS02_SYS = "JSFS02_SYS";
        /// <summary>
        /// 转账支票
        /// </summary>
        public const string JSFS03_SYS = "JSFS03_SYS";
        /// <summary>
        /// 电汇
        /// </summary>
        public const string JSFS04_SYS = "JSFS04_SYS";
        /// <summary>
        /// 信汇
        /// </summary>
        public const string JSFS05_SYS = "JSFS05_SYS";
        /// <summary>
        /// 商业承兑汇票
        /// </summary>
        public const string JSFS06_SYS = "JSFS06_SYS";
        /// <summary>
        /// 银行承兑汇票
        /// </summary>
        public const string JSFS07_SYS = "JSFS07_SYS";
        /// <summary>
        /// 信用证
        /// </summary>
        public const string JSFS08_SYS = "JSFS08_SYS";
        /// <summary>
        /// 应收票据背书
        /// </summary>
        public const string JSFS09_SYS = "JSFS09_SYS";
        /// <summary>
        /// 内部利息结算
        /// </summary>
        public const string JSFS10_SYS = "JSFS10_SYS";
        /// <summary>
        /// 票据退票
        /// </summary>
        public const string JSFS12_SYS = "JSFS12_SYS";
        /// <summary>
        /// 集中结算
        /// </summary>
        public const string JSFS21_SYS = "JSFS21_SYS";
        /// <summary>
        /// 保证金转货款
        /// </summary>
        public const string JSFS22_SYS = "JSFS22_SYS";
        /// <summary>
        /// 微信
        /// </summary>
        public const string JSFS31_SYS = "JSFS31_SYS";
        /// <summary>
        /// 支付宝
        /// </summary>
        public const string JSFS32_SYS = "JSFS32_SYS";
        #endregion

        #region 收款用途（常量）
        /// <summary>
        /// 销售收款
        /// </summary>
        public const string SFKYT01_SYS = "SFKYT01_SYS";
        /// <summary>
        /// 预收款
        /// </summary>
        public const string SFKYT02_SYS = "SFKYT02_SYS";
        /// <summary>
        /// 代收款项
        /// </summary>
        public const string SFKYT04_SYS = "SFKYT04_SYS";
        /// <summary>
        /// 接受捐赠
        /// </summary>
        public const string SFKYT05_SYS = "SFKYT05_SYS";
        /// <summary>
        /// 罚款收入
        /// </summary>
        public const string SFKYT06_SYS = "SFKYT06_SYS";
        /// <summary>
        /// 其他收入
        /// </summary>
        public const string SFKYT07_SYS = "SFKYT07_SYS";
        /// <summary>
        /// 资金下拨
        /// </summary>
        public const string SFKYT22_SYS = "SFKYT22_SYS";
        /// <summary>
        /// 信贷收款
        /// </summary>
        public const string SFKYT24_SYS = "SFKYT24_SYS";
        /// <summary>
        /// 利息收入
        /// </summary>
        public const string SFKYT03_SYS = "SFKYT03_SYS";
        /// <summary>
        /// 资金调拨收款
        /// </summary>
        public const string SFKYT27_SYS = "SFKYT27_SYS";
        /// <summary>
        /// 保证金收入
        /// </summary>
        public const string SFKYT41_SYS = "SFKYT41_SYS";
        #endregion


 
        /// <summary>
        /// 结算方式
        /// 金蝶用key值，不用value，建议用常量
        /// </summary>
        public enum Payment
        {
            /// <summary>
            /// 现金
            /// </summary>
            JSFS01_SYS = 1,
            /// <summary>
            /// 现金支票
            /// </summary>
            JSFS02_SYS = 2,
            /// <summary>
            /// 转账支票
            /// </summary>
            JSFS03_SYS = 3,
            /// <summary>
            /// 电汇
            /// </summary>
            JSFS04_SYS = 4,
            /// <summary>
            /// 信汇
            /// </summary>
            JSFS05_SYS = 5,
            /// <summary>
            /// 商业承兑汇票
            /// </summary>
            JSFS06_SYS = 6,
            /// <summary>
            /// 银行承兑汇票
            /// </summary>
            JSFS07_SYS = 7,
            /// <summary>
            /// 信用证
            /// </summary>
            JSFS08_SYS = 8,
            /// <summary>
            /// 应收票据背书
            /// </summary>
            JSFS09_SYS = 9,
            /// <summary>
            /// 内部利息结算
            /// </summary>
            JSFS10_SYS = 10,
            /// <summary>
            /// 票据退票
            /// </summary>
            JSFS12_SYS = 11,
            /// <summary>
            /// 集中结算
            /// </summary>
            JSFS21_SYS = 12,
            /// <summary>
            /// 保证金转货款
            /// </summary>
            JSFS22_SYS = 13,
            /// <summary>
            /// 微信
            /// </summary>
            JSFS31_SYS = 14,
            /// <summary>
            /// 支付宝
            /// </summary>
            JSFS32_SYS = 15
        }


        #endregion
    }
}
