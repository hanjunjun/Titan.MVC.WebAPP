/************************************************************************
 * 文件名：MyPowerModel
 * 文件功能描述：控制器权限母版
 * 作    者：  hjj
 * 创建日期：2017年6月17日
 * 修 改 人：
 * 修改日期：
 * 修改原因：
 * Copyright (c) 2017 Titan.Han . All Rights Reserved. 
 * ***********************************************************************/
namespace Titan.Controllers.ViewModel.CommonViewModel
{
    public class MyPowerModel
    {
        /// <summary>
        /// 下拉框，变化时是否刷新数据
        /// </summary>
        public int IsQueue { get; set; } = 1;

        /// <summary>
        /// 菜单主键
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// 添加（FunID：1）
        /// </summary>
        public bool Add { get; set; } = false;
        /// <summary>
        /// 修改（FunID：2）
        /// </summary>
        public bool Update { get; set; } = false;
        /// <summary>
        /// 删除（FunID：3）
        /// </summary>
        public bool Delete { get; set; } = false;
        /// <summary>
        /// 查看（FunID：4）
        /// </summary>
        public bool Look { get; set; } = false;
        /// <summary>
        /// 导出（FunID：5）
        /// </summary>
        public bool Export { get; set; } = false;
        /// <summary>
        /// 启用/停用（FunID：6）
        /// </summary>
        public bool StartStop { get; set; } = false;
        /// <summary>
        /// 重置密码（FunID：7）  
        /// </summary>     
        public bool ReSetPassWord { get; set; } = false;
        /// <summary>
        /// 权限分配（FunID：8）
        /// </summary>
        public bool PowerAllot { get; set; } = false;
        /// <summary>
        /// 体检信息（FunID：9）
        /// </summary>
        public bool TJLook { get; set; } = false;
        /// <summary>
        /// 健康趋势（FunID：10）
        /// </summary>
        public bool HealthLook { get; set; } = false;
        /// <summary>
        /// 确诊（FunID：11）
        /// </summary>
        public bool Diagnose { get; set; } = false;
        /// <summary>
        /// 处理（FunID：12）
        /// </summary>
        public bool Handing { get; set; } = false;
        /// <summary>
        /// 解绑（FunID：13）
        /// </summary>
        public bool Unbind { get; set; } = false;
        /// <summary>
        /// 基卫信息（FunID：14）
        /// </summary>
        public bool JWInfo { get; set; } = false;
        /// <summary>
        /// 干预效果（FunID：15）
        /// </summary>
        public bool Intervention { get; set; } = false;
        /// <summary>
        /// 上传体检数据（FunID：16）
        /// </summary>
        public bool PostTJInfo { get; set; } = false;
        /// <summary>
        /// 打印（FunID：17）
        /// </summary>
        public bool Print { get; set; } = false;

        /// <summary>
        /// 销假（FunID：18）
        /// </summary>
        public bool Back { get; set; } = false;

        /// <summary>
        /// 取消（FunID：19）
        /// </summary>
        public bool Logout { get; set; } = false;

        /// <summary>
        /// 已外出（FunID：20）
        /// </summary>
        public bool OutAlready { get; set; } = false;

        /// <summary>
        /// 班次设置（FunID：21）
        /// </summary>
        public bool SetShiftWorker { get; set; } = false;

        /// <summary>
        /// 设置护理类型 （FunID：22）
        /// </summary>
        public bool SetNursingType { get; set; } = false;

        /// <summary>
        /// 机构权限分配（FunID：23）
        /// </summary>
        public bool SetRoleCompanyPower { get; set; } = false;

        /// <summary>
        /// 转入住（FunID：24）
        /// </summary>
        public bool TransferredToLive { get; set; } = false;

        /// <summary>
        /// 入住评估（FunID：25）
        /// </summary>
        public bool CheckInAssessment { get; set; } = false;

        /// <summary>
        /// 入住信息（FunID：26）
        /// </summary>
        public bool CheckInInformation { get; set; } = false;

        /// <summary>
        /// 激活（FunID：27）
        /// </summary>
        public bool Active { get; set; } = false;

        /// <summary>
        /// 挂失（FunID：28）
        /// </summary>
        public bool ReportOfLoss { get; set; } = false;

        /// <summary>
        /// 换卡（FunID：29）
        /// </summary>
        public bool ChangeCard { get; set; } = false;

        /// <summary>
        /// 退卡（FunID：30）
        /// </summary>
        public bool EjectCard { get; set; } = false;

        /// <summary>
        /// 添加下级节 （FunID：31）
        /// </summary>
        public bool AddTheLowerSection { get; set; } = false;

        /// <summary>
        /// 停止服务 （FunID：32）
        /// </summary>
        public bool StopService { get; set; } = false;

        /// <summary>
        /// 长者病历 (FunID:33)
        /// </summary>
        public bool ElderlyRecord { get; set; } = false;

        /// <summary>
        /// 长者医嘱 (FunID:34)
        /// </summary>
        public bool DoctorList { get; set; } = false;

        /// <summary>
        /// 委托用药(FunID:35)
        /// </summary>
        public bool EntrustedDrug { get; set; } = false;

        /// <summary>
        /// 体征记录(FunID:36)
        /// </summary>
        public bool SignRecord { get; set; } = false;

        /// <summary>
        /// 入院登记(FunID:37)
        /// </summary>
        public bool AdmissionRegistration { get; set; } = false;

        /// <summary>
        /// 记录病程(FunID:38)
        /// </summary>
        public bool RecordEdit { get; set; } = false;

        /// <summary>
        /// 开医嘱(FunID:39)
        /// </summary>
        public bool AddDoctor { get; set; } = false;

		/// <summary>
		/// 固定费用(FunID:40)
		/// </summary>
		public bool AddFixedCostDesc { get; set; } = false;

		/// <summary>
		/// 入院登记是否可选(FunID:41)
		/// </summary>
		public bool OtherMonthIsSo { get; set; } = false;

        /// <summary>
        /// 补量(FunID:42)
        /// </summary>
        public bool EntrustGrantFill { get; set; } = false;

        /// <summary>
        /// 核对(FunID:43)
        /// </summary>
        public bool CheckEntrustGrant { get; set; } = false;

        /// <summary>
        /// 确认发药(FunID:44)
        /// </summary>
        public bool CheckEntrustGrantRecord { get; set; } = false;

        /// <summary>
        /// 备份(FunID:45)
        /// </summary>
        public bool BackUp { get; set; } = false;

        /// <summary>
        /// 执行(FunID:46)
        /// </summary>
        public bool Execute { get; set; } = false;

        /// <summary>
        ///  收费 (FunID:47)
        /// </summary>
        public bool Payment { get; set; } = false;

        /// <summary>
        /// 结算(FunID:48)
        /// </summary>
        public bool Settlement { get; set; } = false;

        /// <summary>
        /// 充值(FunID:49)
        /// </summary>
        public bool Recharge { get; set; } = false;

        /// <summary>
        /// 领回(FunID:50)
        /// </summary>
        public bool Reclaim { get; set; } = false;

        /// <summary>
        /// 维护(FunID:51)
        /// </summary>
        public bool Maintain { get; set; } = false;

		/// <summary>
		/// APP权限配置(FunID:52)
		/// </summary>
		public bool AppPowerAllot { get; set; } = false;

        /// <summary>
        /// 定时服务运行(FunID:53)
        /// </summary>
        public bool Run { get; set; } = false;

        /// <summary>
        /// 定时服务停止(FunID:54)
        /// </summary>
        public bool Stop { get; set; } = false;

		/// <summary>
		/// 模板管理标题添加(FunID:55)
		/// </summary>
		public bool ModuleMTAdd { get; set; } = false;

		/// <summary>
		/// 模板管理标题编辑(FunID:56)
		/// </summary>
		public bool ModuleMTEdit { get; set; } = false;

		/// <summary>
		/// 模板管理标题删除(FunID:57)
		/// </summary>
		public bool ModuleMTDelete { get; set; } = false;

		/// <summary>
		/// 模板管理内容添加(FunID:58)
		/// </summary>
		public bool ModuleMCAdd { get; set; } = false;

		/// <summary>
		/// 模板管理内容编辑(FunID:59)
		/// </summary>
		public bool ModuleMCEdit { get; set; } = false;

		/// <summary>
		/// 模板管理内容删除(FunID:60)
		/// </summary>
		public bool ModuleMCDelete { get; set; } = false;

		/// <summary>
		/// 模板管理内容查看(FunID:61)
		/// </summary>
		public bool ModuleMCLook { get; set; } = false;

        /// <summary>
        /// 查看服务执行日志(FunID:62)
        /// </summary>
        public bool QuartzLookLog { get; set; } = false;

        /// <summary>
        /// 服务立即执行一次(FunID:63)
        /// </summary>
        public bool QuartzRunOnce { get; set; } = false;

        /// <summary>
        /// 交班(FunID:64)
        /// </summary>
        public bool NextShift { get; set; } = false;

        /// <summary>
        /// 记录(FunID:65)
        /// </summary>
        public bool Record { get; set; } = false;

        /// <summary>
        /// 审核(FunID:66)
        /// </summary>
        public bool Audit { get; set; } = false;

        /// <summary>
        /// 提交(FunID:67)
        /// </summary>
        public bool Submit { get; set; } = false;

        /// <summary>
        /// 确认金额(FunID:68)
        /// </summary>
        public bool Confirm { get; set; } = false;

        /// <summary>
        /// 上传凭证(FunID:69)
        /// </summary>
        public bool Upload { get; set; } = false;

        /// <summary>
        /// 发放(FunID:70)
        /// </summary>
        public bool Grant { get; set; } = false;

        /// <summary>
        /// 包房(FunID:71)
        /// </summary>
        public bool BaoF { get; set; } = false;

        /// <summary>
        /// 长者档案(FunID:72)
        /// </summary>
        public bool Elder { get; set; } = false;

        /// <summary>
        /// 居住档案(FunID:73)
        /// </summary>
        public bool Live { get; set; } = false;

        /// <summary>
        /// 评估档案(FunID:74)
        /// </summary>
        public bool Assessment { get; set; } = false;

        /// <summary>
        /// 医疗档案(FunID:75)
        /// </summary>
        public bool Medical { get; set; } = false;

        /// <summary>
        /// 财务档案(FunID:76)
        /// </summary>
        public bool Finance { get; set; } = false;

        /// <summary>
        /// 社工服务档案(FunID:77)
        /// </summary>
        public bool Service { get; set; } = false;

        /// <summary>
        /// 处方依据(FunID:78)
        /// </summary>
        public bool EntrustImg { get; set; } = false;

        /// <summary>
        /// 委托操作(FunID:79)
        /// </summary>
        public bool EntrustGrantHandle { get; set; } = false;

        /// <summary>
        /// 出院小结(FunID:80)
        /// </summary>
        public bool OutRecord { get; set; } = false;

        /// <summary>
        /// 服药单(FunID:81)
        /// </summary>
        public bool TakeMedical { get; set; } = false;

        /// <summary>
        /// 输液单(FunID:82)
        /// </summary>
        public bool Infusion { get; set; } = false;

        /// <summary>
        /// 治疗单(FunID:83)
        /// </summary>
        public bool Cure { get; set; } = false;

        /// <summary>
        /// 补贴材料(FunID:84)
        /// </summary>
        public bool SubsidiesMaterials { get; set; } = false;

        /// <summary>
        /// 未执行(FunID:85)
        /// </summary>
        public bool NoExecute { get; set; } = false;

        /// <summary>
        /// 模板下载(FunID:86)
        /// </summary>
        public bool DownLoad { get; set; } = false;

        /// <summary>
        /// 导入(FunID:87)
        /// </summary>
        public bool Import { get; set; } = false;

        /// <summary>
        /// 审核(FunID:88)
        /// </summary>
        public bool Examine { get; set; } = false;

        /// <summary>
        /// 长者主页(FunID:89)
        /// </summary>
        public bool ElderIndex { get; set; } = false;

        /// <summary>
        /// 补贴报销(FunID:90)
        /// </summary>
        public bool Subsidy { get; set; } = false;

        /// <summary>
        /// 初始费用(FunID:91)
        /// </summary>
        public bool InitialFee { get; set; } = false;

        /// <summary>
        /// 预交费(FunID:92)
        /// </summary>
        public bool PrePayment { get; set; } = false;

        /// <summary>
        /// 月度缴费(FunID:93)
        /// </summary>
        public bool MonthCost { get; set; } = false;

        /// <summary>
        /// 欠费管理(FunID:94)
        /// </summary>
        public bool OwingCost { get; set; } = false;

        /// <summary>
        /// 退住结算(FunID:95)
        /// </summary>
        public bool RetreatCost{ get; set; } = false;

        /// <summary>
        /// 回访(FunID:96)
        /// </summary>
        public bool Review { get; set; } = false;

        /// <summary>
        /// 评估(FunID:97)
        /// </summary>
        public bool Assess { get; set; } = false;

        /// <summary>
        /// 缴费通知单(FunID:98)
        /// </summary>
        public bool PaymentNotice { get; set; } = false;

        /// <summary>
        /// 签约(FunID:99)
        /// </summary>
        public bool AgreementSign { get; set; } = false;

        /// <summary>
        /// 杨浦委托医嘱(FunID:100)
        /// </summary>
        public bool YPEntrustGrand { get; set; } = false;

        /// <summary>
        /// 杨浦委托库存(FunID:101)
        /// </summary>
        public bool YPEntrustDrugStock { get; set; } = false;

        /// <summary>
        /// 杨浦委托医嘱新增(FunID:102)
        /// </summary>
        public bool YPEntrustAdd { get; set; } = false;

        /// <summary>
        /// 杨浦委托医嘱启用停用(FunID:103)
        /// </summary>
        public bool YPEntrustStartStop { get; set; } = false;

        /// <summary>
        /// 杨浦委托医嘱调整(FunID:104)
        /// </summary>
        public bool YPEntrustEdit { get; set; } = false;


        /// <summary>
        /// 杨浦委托库存增加(FunID:105)
        /// </summary>
        public bool YPEntrustDrugStockAdd { get; set; } = false;

        /// <summary>
        /// 费用补录(FunID:106)
        /// </summary>
        public bool AdditionRecord { get; set; } = false;

        /// <summary>
        /// 费用补录-新增(FunID:107)
        /// </summary>
        public bool AdditionRecordAdd { get; set; } = false;

        /// <summary>
        /// 费用补录-编辑(FunID:108)
        /// </summary>
        public bool AdditionRecordEdit { get; set; } = false;

        /// <summary>
        /// 费用补录-删除(FunID:109)
        /// </summary>
        public bool AdditionRecordDelete { get; set; } = false;

        /// <summary>
        /// 费用补录-查看(FunID:110)
        /// </summary>
        public bool AdditionRecordLook { get; set; } = false;

        /// <summary>
        /// 通知管理（民政）-查看回复(FunID:111)
        /// </summary>
        public bool LookReply { get; set; } = false;

        /// <summary>
        /// 通知管理（民政）-发布(FunID:112)
        /// </summary>
        public bool Release { get; set; } = false;

        /// <summary>
        /// 通知管理（机构）-回复(FunID:113)
        /// </summary>
        public bool Reply { get; set; } = false;
    }
}