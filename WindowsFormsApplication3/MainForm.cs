using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSet2ExcelDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var ctx = new jointexamEntities();
            List<view_result> rs = ctx.view_result.ToList();

            Dictionary<String, Dictionary<String, Dictionary<int, List<view_result>>>> dic = new Dictionary<string, Dictionary<string, Dictionary<int, List<view_result>>>>();

            foreach (view_result r in rs)
            {
               if(!dic.ContainsKey(r.性别)){
                    dic.Add(r.性别,new Dictionary<string,Dictionary<int, List<view_result>>>());
                }

               if (!dic[r.性别].ContainsKey(r.年龄分组))
               {
                   dic[r.性别].Add(r.年龄分组, new Dictionary<int,  List<view_result>>());
               }

               if (!dic[r.性别][r.年龄分组].ContainsKey(r.analysis_id))
               {
                   dic[r.性别][r.年龄分组].Add(r.analysis_id, new List<view_result>());
               }

               dic[r.性别][r.年龄分组][r.analysis_id].Add(r);
            }


            foreach (string sex in dic.Keys)
            {
                foreach (String ageGroup in dic[sex].Keys)
                {
                    foreach (int aname in dic[sex][ageGroup].Keys)
                    {
                        dic[sex][ageGroup][aname].Sort(new CC());
                    }
                }
            }

            foreach (string sex in dic.Keys)
            {
                foreach (String ageGroup in dic[sex].Keys)
                {
                    foreach (int aname in dic[sex][ageGroup].Keys)
                    {
                        String num = dic[sex][ageGroup][aname].Count.ToString();
                        String min = dic[sex][ageGroup][aname][dic[sex][ageGroup][aname].Count - 1].结果值.ToString();
                        String max = dic[sex][ageGroup][aname][0].结果值.ToString();
                        int seq = 1;
                        foreach (view_result r in dic[sex][ageGroup][aname])
                        {
                             
                            DataSet1.RDLCTable.AddRDLCTableRow(r.客户号, r.姓名, r.性别, (int)r.测试年龄, (DateTime)r.生日, r.种族, (double)r.身高,
                                (double)r.体重, r.测试动作, r.分析类型, r.分析名称, r.结果值.ToString() , r.X.ToString(), r.Y.ToString(), r.Z.ToString(), r.年龄分组,num,min,max,seq.ToString(),r.结果类型);
                            seq++;
                        }

                       
                    }

                    ExcelWriter.CreateExcel(DataSet1.RDLCTable, "c:\\" + ageGroup + sex + ".xlsx");

                    DataSet1.RDLCTable.Clear();
                }
            }


           /// this.reportViewer1.RefreshReport();

           
        }

       

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }

    class CC : IComparer<view_result>
    {

        public int Compare(view_result t1, view_result t2)
        {
            if (t1.结果值 > t2.结果值)
            {
                return -1;
            }
            else if (t1.结果值 < t2.结果值)
            {
                return 1;
            }
            else
                return 0;
        }
    }
}
