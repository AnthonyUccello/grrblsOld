using UnityEngine;
using System.Collections;
using System; 
using System.Data; 
using System.Data.Odbc; 

public class EXCELREADER : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		readXLS(Application.dataPath + "/Book1.xls");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void readXLS( string filetoread)
	{
		// Must be saved as excel 2003 workbook, not 2007, mono issue really
		string driver = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq="+filetoread+";";
		string queryCommand = "SELECT * FROM [Sheet1$]"; 
		// our odbc connector 
		OdbcConnection odbcConnector = new OdbcConnection(driver); 
		// our command object 
		OdbcCommand odbcCommand = new OdbcCommand(queryCommand, odbcConnector);
		// table to hold the data 
		DataTable dataTable = new DataTable("YourData"); 
		// open the connection 
		odbcConnector.Open(); 
		// lets use a datareader to fill that table! 
		OdbcDataReader dataReader = odbcCommand.ExecuteReader(); 
		// now lets blast that into the table by sheer man power! 
		dataTable.Load(dataReader); 
		// close that reader! 
		dataReader.Close(); 
		// close your connection to the spreadsheet! 
		odbcConnector.Close(); 
		// wow look at us go now! we are on a roll!!!!! 
		// lets now see if our table has the spreadsheet data in it, shall we? 
		
		if(dataTable.Rows.Count > 0) 
		{ 
			// do something with the data here 
			// but how do I do this you ask??? good question! 
			for (int i = 0; i < dataTable.Rows.Count; i++) 
			{ 
				// for giggles, lets see the column name then the data for that column! 
				/*Debug.Log(dtYourData.Columns[0].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[0].ColumnName].ToString() + 
				          "  |  " + dtYourData.Columns[1].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[1].ColumnName].ToString() + 
				          "  |  " + dtYourData.Columns[2].ColumnName + " : " + dtYourData.Rows[i][dtYourData.Columns[2].ColumnName].ToString()); */
				Debug.Log("C 1 " + dataTable.Columns[0].ColumnName);
			} 
		} 
	}
}