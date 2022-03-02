using BPM;
using BPM.Client;
using BPMServicesAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;

namespace BPMServicesAPI.Controllers
{
    public class DefaultController : ApiController
    {
        /// <summary>
        /// 退回重填
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [Route("api/RecedeRestart")]
        [HttpPost]
        public int RecedeRestart(BPMModels models)
        {
            using (BPMConnection cn = new BPMConnection())
            {
                cn.Open(models.BPMServerIP, models.BPMUser, models.BPMUserPass, models.BPMServerPort);
                BPMTask.RecedeRestart(cn, models.TaskId, models.Comments);
            }
            return 0;
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [Route("api/Reject")]
        [HttpPost]
        public int Reject(BPMModels models)
        {
            using (BPMConnection cn = new BPMConnection())
            {
                cn.Open(models.BPMServerIP, models.BPMUser, models.BPMUserPass, models.BPMServerPort);
                BPMTask.Reject(cn, models.TaskId, models.Comments);
            }
            return 0;
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [Route("api/approve")]
        [HttpPost]
        public int Approve(BPMModels models)
        {
            int TaskID = 0;
            PostResult result = null;
            using (BPMConnection cn = new BPMConnection())
            {
                //Version version = cn.GetGlobalObjectLastVersion(StoreZoneType.Process, models.ProcessName);
                //MemberCollection positions = OrgSvr.GetUserPositions(cn, models.FullName);
                //string FullName = positions[0].FullName;
                //PostInfo info = BPMProcess.GetPostInfo(cn, models.ProcessName, version, FullName);
                cn.Open(models.BPMServerIP, models.BPMUser, models.BPMUserPass, models.BPMServerPort);
                result = BPMProcStep.Approve(cn, models.StepId, models.Comments, true);
            }
            TaskID = result.TaskID;
            return TaskID;
        }
        //发起流程
        [Route("api/StartBPM")]
        [HttpPost]
        public int startBPM(BPMModels models)
        {
            int taskid = 0;
            using (BPMConnection cn = new BPMConnection())
            {

                cn.Open(models.BPMServerIP, models.BPMUser, models.BPMUserPass, models.BPMServerPort);

                MemberCollection positions = OrgSvr.GetUserPositions(cn, models.FullName);
                string FullName = positions[0].FullName;


                TableIdentityCollection tableIdentityCollection = BPM.Client.BPMProcess.GetProcessGlobalTableIdentitys(cn, models.ProcessName);
                FlowDataSet FD = DataSourceManager.LoadDataSetSchema(cn, tableIdentityCollection);

                //设置Header
                DataTable tableHeader = new DataTable("Header");
                tableHeader.Columns.Add(new DataColumn("Method", typeof(string)));
                tableHeader.Columns.Add(new DataColumn("ProcessName", typeof(string)));
                tableHeader.Columns.Add(new DataColumn("Action", typeof(string)));
                tableHeader.Columns.Add(new DataColumn("OwnerMemberFullName", typeof(string)));
                DataRow rowHeader = tableHeader.NewRow();
                //设置Header数据
                rowHeader["Method"] = "Post";
                rowHeader["ProcessName"] = models.ProcessName;
                rowHeader["Action"] = models.Action;
                rowHeader["OwnerMemberFullName"] = FullName;
                tableHeader.Rows.Add(rowHeader);

                //////////////////////////////////////////
                //生成XML
                StringBuilder sb = new StringBuilder();
                StringWriter w = new StringWriter(sb);

                w.WriteLine("<?xml version=\"1.0\"?>");
                w.WriteLine("<XForm>");
                tableHeader.WriteXml(w, XmlWriteMode.IgnoreSchema, false);
                w.WriteLine(models.FormDataSet);
                w.WriteLine("</XForm>");
                w.Close();


                String xmlData = sb.ToString();
                xmlData = xmlData.Replace("<DocumentElement>", "");
                xmlData = xmlData.Replace("</DocumentElement>", "");

                MemoryStream xmlStream = new MemoryStream(UTF8Encoding.UTF8.GetBytes(xmlData));

                PostResult result = BPMProcess.Post(cn, xmlStream);
                taskid = result.TaskID;

            }
            return taskid;
        }
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [Route("api/GetUrl")]
        [HttpPost]
        public string GetPostUrl(BPMModels models)
        {
            PostInfo info = null;
            using (BPMConnection cn = new BPMConnection())
            {
                cn.Open(models.BPMServerIP, models.BPMUser, models.BPMUserPass, models.BPMServerPort);
                Version version = cn.GetGlobalObjectLastVersion(StoreZoneType.Process, models.ProcessName);
                MemberCollection positions = OrgSvr.GetUserPositions(cn, models.FullName);
                string FullName = positions[0].FullName;
                info = BPMProcess.GetPostInfo(cn, models.ProcessName, version, FullName);
            }
            return info.FormFile;
        }
    }
}
