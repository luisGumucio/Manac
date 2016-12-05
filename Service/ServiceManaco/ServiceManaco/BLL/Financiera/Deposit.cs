using System;
using System.Data;
using System.Data.Common;
using Bata.Aquarella.BLL.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Bata.Aquarella.BLL
{
    public class Deposit
    {
        public static string _conn = Constants.OrcleStringConn;

        public static bool saveDeposits(string _co, string _bank, string _noConsig, DateTime _datePay, decimal _amount, string _warehouse)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            bool result = false;
            string sqlCommand = "FINANCIERA.SP_ADD_DEPOSITS";
            string _Payment_Id = string.Empty;
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co,
                 _Payment_Id, _bank, _noConsig, _datePay, _amount, _warehouse);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    _Payment_Id = Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_dev_deposit_id"));

                    // Commit the transaction.
                    transaction.Commit();

                    result = true;
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    connection.Close();
                    //throw new Exception(e.Message, e.InnerException);
                }
                connection.Close();
                return result;
            }
        }

        static public DataSet getDeposits(string _co, int _warehouse,DateTime _startDate, DateTime _endDate)
        {
            try
            {
                // CURSOR REF
                object results = new object[1];

                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(_conn);

                string sqlCommand = "FINANCIERA.SP_LOADDEPOSITS";
                DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _co, _warehouse, _startDate, _endDate, results);
                // DataSet that will hold the returned results		
                // Note: connection closed by ExecuteDataSet method call 
                return db.ExecuteDataSet(dbCommandWrapper);
            }
            catch (Exception e) { throw new Exception(e.Message, e.InnerException); }
        }

        public static bool deleteDeposits(string _depositsId)
        {
            Database db = DatabaseFactory.CreateDatabase(_conn);
            bool result = false;
            string sqlCommand = "FINANCIERA.SP_DELETE_DEPOSITS";
            string _Payment_Id = string.Empty;
            DbCommand dbCommandWrapper = db.GetStoredProcCommand(sqlCommand, _depositsId);
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                try
                {
                    db.ExecuteNonQuery(dbCommandWrapper, transaction);
                    //_Payment_Id = Convert.ToString(db.GetParameterValue(dbCommandWrapper, "p_dev_deposit_id"));

                    // Commit the transaction.
                    transaction.Commit();

                    result = true;
                }
                catch (Exception e)
                {
                    // Roll back the transaction. 
                    transaction.Rollback();
                    connection.Close();
                    //throw new Exception(e.Message, e.InnerException);
                }
                connection.Close();
                return result;
            }
        }

    }
}