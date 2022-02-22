using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BPMServicesAPI.Models
{
    public class BPMModels
    {
        /// <summary>
        /// 流程名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 添加时需要的提交动作 看发起流程线上的文字来定义
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 发起流程人员
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// bpm服务器IP
        /// </summary>
        public string BPMServerIP { get; set; }
        /// <summary>
        /// 登录服务器密码，登录平台的密码
        /// </summary>
        public string BPMUserPass { get; set; }
        /// <summary>
        /// 登录服务器的用户名或密码
        /// </summary>
        public string BPMUser { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int BPMServerPort { get; set; }
        /// <summary>
        /// 传递到流程的数据
        /// </summary>
        public string FormDataSet { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// 流程节点ID
        /// </summary>
        public int StepId { get; set; }
        /// <summary>
        /// 意见
        /// </summary>
        public string Comments { get; set; }
    }
}