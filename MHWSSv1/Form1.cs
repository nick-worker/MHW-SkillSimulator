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
        IDictionary<String, IDictionary<int,String>> skillMap;
        IDictionary<int, String> skillLevelDesc;
        IDictionary<String, int> selectedSkill;
        string selectedSkillName;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SQLTest.createTableTest();

            skillMap = new Dictionary<String, IDictionary<int, String>>();
            skillLevelDesc = new Dictionary<int, String>();
            selectedSkill = new Dictionary<String, int>();

            // ウィンドウタイトルの設定
            this.Text = "MHWSS v1";
            // ボタン1のテキスト設定
            button1.Text = "追加";
            // スキルツリーXML読み込み
            ImportSkillXml("../../xml/SkillList/mhwss-skill.xml");
            // スキル詳細XML読み込み
            ImportSkillDescXml("../../xml/SkillList/mhwss-skill-desc.xml");
            // 選択済みスキルのグリッドビュー設定
            SettingDataGridView1();
        }

        /*
         * スキル一覧のXMLをロード
         * 
         */
        private void ImportSkillXml(string xmlPath)
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

            // 再帰的にスキルツリーを構成
            SettingSkillTree(rootXmlNode, rootTreeNode);

        }

        /*
         * スキル選択のツリーを設定
         * 
         */
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
                        case "skillgroup":
                            sni.Type = "skillgroup";
                            break;
                        case "skill":
                            sni.Type = "skill";
                            break;
                        default:
                            sni.Type = null;
                            break;
                    }
                    // name属性を取得
                    sni.Name = ((XmlElement)childXmlNode).GetAttribute("name");
                    // limit属性を取得
                    sni.Limit = ((XmlElement)childXmlNode).GetAttribute("limit");
                    // need属性を取得
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

        /*
         * Skill説明XMLをロード
         * 
         */
        private void ImportSkillDescXml(string xmlPath)
        {
            // XML読み込み
            XmlDocument importXmlDocument = new XmlDocument();
            importXmlDocument.Load(xmlPath);

            // ルートノード取得
            XmlNode rootXmlNode = importXmlDocument.DocumentElement;


            SkillNodeInfo sni = new SkillNodeInfo();
            sni.Type = ((XmlElement)rootXmlNode).Name;
            sni.Name = ((XmlElement)rootXmlNode).GetAttribute("name");

            // 再帰的にスキルツリーを構成
            SettingSkillDescription(rootXmlNode);

            skillMap.Add(sni.Name, skillLevelDesc);

        }

        /*
         * Skill説明XMLをクラス変数（Map）にセット
         * 
         */
        private void SettingSkillDescription(XmlNode Parentnode)
        {
            foreach(XmlNode childXmlNode in Parentnode.ChildNodes)
            {
                if(childXmlNode.Name != "#text")
                {
                    SkillNodeInfo sni = new SkillNodeInfo();

                    //タグ名を取得
                    switch (childXmlNode.Name)
                    {
                        case "skill":
                            sni.Type = "skill";
                            break;
                        case "item":
                            sni.Type = "item";
                            break;
                        default:
                            sni.Type = null;
                            break;
                    }
                    // level属性を取得
                    sni.Level = ((XmlElement)childXmlNode).GetAttribute("level");
                    // desc属性を取得
                    sni.Desc = ((XmlElement)childXmlNode).GetAttribute("desc");

                    skillLevelDesc.Add(int.Parse(sni.Level), sni.Desc);

                    // 再帰呼び出し
                    SettingSkillDescription(childXmlNode);
                }
            }
        }
        
        /*
         * DataGridView1 のセッティング
         * 
         */
        private void SettingDataGridView1()
        {
            // セルの読み取り専用
            dataGridView1.ReadOnly = true;

            // Numberカラムの設定
            DataGridViewTextBoxColumn textColumn1 = new DataGridViewTextBoxColumn();
            textColumn1.DataPropertyName = "Column1";
            textColumn1.Name = "Number";
            textColumn1.HeaderText = "No";
            textColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(textColumn1);

            // Skillカラムの設定
            DataGridViewTextBoxColumn textColumn2 = new DataGridViewTextBoxColumn();
            textColumn2.DataPropertyName = "Column2";
            textColumn2.Name = "Skill";
            textColumn2.HeaderText = "スキル";
            textColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(textColumn2);

            // Levelカラムの設定
            DataGridViewTextBoxColumn textColumn4 = new DataGridViewTextBoxColumn();
            textColumn4.DataPropertyName = "Column4";
            textColumn4.Name = "Level";
            textColumn4.HeaderText = "Lv";
            textColumn4.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(textColumn4);

            // Descriptionカラムの設定
            DataGridViewTextBoxColumn textColumn5 = new DataGridViewTextBoxColumn();
            textColumn5.DataPropertyName = "Column5";
            textColumn5.Name = "Description";
            textColumn5.HeaderText = "備考";
            textColumn5.SortMode = DataGridViewColumnSortMode.NotSortable;
            textColumn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(textColumn5);

            // Valueカラムの設定
//            DataGridViewTextBoxColumn textColumn3 = new DataGridViewTextBoxColumn();
//            textColumn3.DataPropertyName = "Column3";
//            textColumn3.Name = "Value";
//            textColumn3.HeaderText = "値";
//            textColumn3.SortMode = DataGridViewColumnSortMode.NotSortable;
//            textColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
//            dataGridView1.Columns.Add(textColumn3);
            dataGridView1.RowCount = 20;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // 行ヘッダ非表示
            dataGridView1.RowHeadersVisible = false;
        }

        /*
         * treeViewで選択したスキルをlistBoxに表示
         * 
         */
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listBox1.Items.Clear();

            // treeviewからスキル情報を取得
            SkillNodeInfo treeSni = (SkillNodeInfo)treeView1.SelectedNode.Tag;
            IDictionary<int, String> skillLevelDescMap = new Dictionary<int, String>();

            // itemの情報のみlistBox1にセット
            if(treeSni.Type == "skill")
            {
                // 選択可能なスキルレベル分行を追加
                int limit = int.Parse(treeSni.Limit);
                for(int i = 0; i < limit; i++)
                {
                    // int j = limit - i;
                    int j = i + 1;
                    listBox1.Items.Add(treeSni.Name + " Lv." + j);
                }
                selectedSkillName = treeSni.Name;
            }
        }

        /*
         * 追加ボタン押下時の動作
         * 
         */
        private void button1_Click(object sender, EventArgs e)
        {
            // listBox1で値が選択されていれば
            if(listBox1.SelectedItem != null)
            {
                // listBoxのIndex + 1 = スキルレベルとする
                int listIndex = listBox1.SelectedIndex;
                int skillLevel = listIndex + 1;

                
                //string selectSkill = listBox1.SelectedItem.ToString();

                // Skill descriptionがあれば取得
                IDictionary<int, String> levelDesc = new Dictionary<int, String>();
                string desc = null;
                if (skillMap.ContainsKey(selectedSkillName))
                {
                    levelDesc = skillMap[selectedSkillName];
                    desc = levelDesc[skillLevel];
                }
                
                //int level = 7;

                dataGridView1[0, 0].Value = "1";
                dataGridView1[1, 0].Value = selectedSkillName;
                dataGridView1[2, 0].Value = skillLevel;
                dataGridView1[3, 0].Value = desc;
            }

        }
    }
}
