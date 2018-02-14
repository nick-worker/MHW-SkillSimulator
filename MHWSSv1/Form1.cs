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
            ImportSkillXml("../../xml/SkillList/mhwss-skill.xml");
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

    }
}
