using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPx.WebCrawler
{
    public static class NavigationResultExtensions
    {
        public static DataTable GetTable(this NavigationResult page, string tableId)
        {
            //var browser = (SHDocVw.WebBrowser)webBrowser1.ActiveXInstance;
            //var document = (mshtml.HTMLDocument)browser.Document;
            var document = page.Document;
            var table = document.GetElementbyId(tableId);
            DataTable dt = new DataTable();
            DataRow dr = null;
            Action<string> addColumn = columnName => dt.Columns.Add(columnName);
            Action<int, string> setValue = (index, value) => (dr = dr ?? dt.NewRow())[index] = value;
            Action<HtmlAgilityPack.HtmlNode> processRow = null;
            processRow = (row) => {
                switch (row.Name)
                {
                    case "thead":
                    case "tbody":
                        foreach (var childRow in row.ChildNodes)
                            if (childRow.Name != "#text")
                                processRow(childRow);
                        return;
                }
                dr = null;
                int cellIndex = 0;
                foreach (var cell in row.ChildNodes)
                    if (cell.Name != "#text")
                    {
                        switch (row.ParentNode.Name)
                        {
                            case "thead":
                                addColumn(cell.InnerText.Trim());
                                break;
                            case "tbody":
                            default:
                                switch (cell.Name)
                                {
                                    case "th": addColumn(cell.InnerText.Trim()); break;
                                    case "td": setValue(cellIndex, cell.InnerText.Trim()); break;
                                }
                                break;
                        }
                        cellIndex++;
                    }
                if (dr != null)
                    dt.Rows.Add(dr);
            };
            foreach (var row in table.ChildNodes)
                if (row.Name != "#text")
                    processRow(row);
            return dt;
        }
    }
}
