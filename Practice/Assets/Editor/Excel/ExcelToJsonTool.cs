using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using UnityEditor;
using UnityEngine;

public class ExcelToJsonTool : Editor
{
    private static string EXCEL_PATH = Application.dataPath +"/ArtRes/Excel/";
    private static string JSON_PATH = Application.streamingAssetsPath +"/";
    private static string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";
    private static List<string> names = new List<string>();
    [MenuItem("GameTool/ExcelToJson")]
    private static void ExcleToJson()
    {
        DirectoryInfo directoryInfo = Directory.CreateDirectory(EXCEL_PATH);
        FileInfo[] fileinfos = directoryInfo.GetFiles();

        DataTableCollection dataTable;
        //处理掉所有后缀名不是xls,xlsx的文件
        for (int i = 0; i < fileinfos.Length; i++)
        {
            if (fileinfos[i].Extension != ".xls" &&
            fileinfos[i].Extension != ".xlsx")
                continue;
            //读取excel文件
            using (FileStream fs = fileinfos[i].Open(FileMode.Open, FileAccess.Read))
            {
                //从数据流中读取Excel文件
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                //得到Excel中的表数据
                dataTable = excelDataReader.AsDataSet().Tables;
                fs.Close();
            }
            //遍历所有表
            foreach (DataTable table in dataTable)
            {
                names.Clear();
                Debug.Log(table.TableName);
                //根据表信息生成json文件
                CreateJson(table);
                //根据表信息生成对应的数据结构类
                //生成数据结构类
                GenerateExcelDataClass(table);
            }
        }
        AssetDatabase.Refresh();
    }
    public static void GenerateExcelDataClass(DataTable table)
    {
        //寻找指定路径文件夹
        if (!Directory.Exists(DATA_CLASS_PATH))
        {
            Directory.CreateDirectory(DATA_CLASS_PATH);
        }
        DataRow rowName = GetVariableNameRow(table);
        DataRow rowType = GetVariableTypeRow(table);

        string str = "public class " + table.TableName + "\n{\n";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            str += "    public " + rowType[i] + " " + rowName[i] + ";\n";
        }
        str += "}";

        //将按规则拼接好的字符串写入
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);
    }

    public static void CreateJson(DataTable table)
    {
        if(!Directory.Exists(JSON_PATH))
        {
            Directory.CreateDirectory(JSON_PATH);
        }
        //***进行字符串拼接***
        //获得字段名称
        DataRow rowName = GetVariableNameRow(table);
        //先遍历所有字段名称放入一个数组中
        for(int i = 0;i<table.Columns.Count;i++)
        {
            names.Add(rowName[i] as string);
        }
        string str = "[";
        //遍历每一行
        for (int i = 3; i < table.Rows.Count; i++)
        {
            str += "{";
            //获得第i行的Excel信息
            DataRow row = table.Rows[i];
            //遍历每一列进行字段赋值的字符串拼接
            for(int j=0;j<table.Columns.Count;j++)
            {
                
                if(j == table.Columns.Count-1)
                {
                    str +=  "\"" + names[j] +"\"" + " : " + row[j].ToString();
                }
                else
                    str += "\"" + names[j] + "\"" + " : " + row[j].ToString() + ",";
            }
            if(i == table.Rows.Count - 1)
            {
                str += "}";
                break;
            }
                str += "},";
        }
        str +="]";
        //将按规则拼接好的字符串写入指定路径
        File.WriteAllText(JSON_PATH + table.TableName + ".json",str);
    }
    /// <summary>
    /// 获取变量名所在行
    /// </summary>
    /// <param name="table"></param>
    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }
    /// <summary>
    /// 获得变量类型所在行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }
    /// <summary>
    /// 获得key行
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    private static DataRow GetKeyRow(DataTable table)
    {
        return table.Rows[2];
    }
    /// <summary>
    /// 获取注释所在行
    /// </summary>
    /// <param name="table"></param>
    private static DataRow GetCommentRow(DataTable table)
    {
        return table.Rows[3];
    }

}
