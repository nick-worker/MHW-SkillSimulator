using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MHWSSv1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "MHWSS v1";
            button1.Text = "追加";
            ImportSkillXml("../../xml/SkillList/mhwss-skill.xml");
            SettingDataGridView1();
        }

        private void ImportSkillXml(String xmlPath)
        {
            // XML読み込み
            XmlDocument importXmlDocument = new XmlDocument();
            importXmlDocument.Load(xmlPath);

            // ルートノード取得
            XmlNode rootXmlNode = importXmlDocument.DocumentElement;


            SkillNodeInfo sni = new SkillNodeInfo();
            sni.Type = ((XmlElement)rootXmlNode).Name;
            sni.Name = ((XmlElement)rootXmlNode).GetAttribute("name");

            // TreeNodeのルートノード生成
            TreeNode rootTreeNode = new TreeNode(sni.Name);
            rootTreeNode.Tag = sni;

            // TreeViewにルートノード追加
            treeView1.Nodes.Add(rootTreeNode);

            SettingSkillTree(rootXmlNode, rootTreeNode);

        }
        private void SettingSkillTree(XmlNode Parentnode, TreeNode ParenttreeNode)
        {
            foreach (XmlNode childXmlNode in Parentnode.ChildNodes) 
            {
                if(childXmlNode.Name != "#text")
                {
                    SkillNodeInfo sni = new SkillNodeInfo();

                    // タグ名を取得
                    switch (childXmlNode.Name)
                    {
                        case "group":
                            sni.Type = "group";
                            break;
                        case "item":
                            sni.Type = "item";
                            break;
                        default:
                            sni.Type = null;
                            break;
                    }
                    // name属性を取得
                    sni.Name = ((XmlElement)childXmlNode).GetAttribute("name");
                    sni.Limit = ((XmlElement)childXmlNode).GetAttribute("limit");
                    sni.Need = ((XmlElement)childXmlNode).GetAttribute("need");

                    // TreeNodeを作成
                    TreeNode tn = new TreeNode(sni.Name);

                    tn.Tag = sni;
                                        
                    // 親ノードに子ノードを追加
                    ParenttreeNode.Nodes.Add(tn);

                    //再帰呼び出し
                    SettingSkillTree(childXmlNode, tn);

                }
            }
        }

        // DataGridView1 のセッティング
        private void SettingDataGridView1()
        {
            dataGridView1.ReadOnly = true;
            DataGridViewTextBoxColumn textColumn1 = new DataGridViewTextBoxColumn();
            textColumn1.DataPropertyName = "Column1";
            textColumn1.Name = "Number";
            textColumn1.HeaderText = "No";
            textColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(textColumn1);

            DataGridViewTextBoxColumn textColumn2 = new DataGridViewTextBoxColumn();
            textColumn2.DataPropertyName = "Column2";
            textColumn2.Name = "Skill";
            textColumn2.HeaderText = "スキル";
            textColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(textColumn2);

            DataGridViewTextBoxColumn textColumn3 = new DataGridViewTextBoxColumn();
            textColumn3.DataPropertyName = "Column3";
            textColumn3.Name = "Value";
            textColumn3.HeaderText = "値";
            textColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(textColumn3);
            //dataGridView1.Row
            dataGridView1.RowCount = 10;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dataGridView1.RowHeadersVisible = false;
        }

        // treeViewで選択したスキルをlistBoxに表示
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listBox1.Items.Clear();
            SkillNodeInfo sni = (SkillNodeInfo)treeView1.SelectedNode.Tag;
            
            // itemの情報のみlistBox1にセット
            if(sni.Type == "item")
            {
                int limit = int.Parse(sni.Limit);
                for(int i = 0; i < limit; i++)
                {
                    int j = limit - i;
                    listBox1.Items.Add(sni.Name + "+" + j);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem != null)
            {
                String selectSkill = listBox1.SelectedItem.ToString();
                dataGridView1[0, 0].Value = "1";
                dataGridView1[1, 0].Value = selectSkill;
            }

        }
    }
}
