using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Security;
using System.Xml;

namespace HRM.Repository
{
		
	///<exclude />
	public sealed class TransactionHelper : IComparable, IComparable<TransactionHelper>, IEquatable<TransactionHelper>
	{
		///<exclude />
		public readonly string ConnectionString;
		///<exclude />
		public readonly SqlTransaction Transaction;
		///<exclude />
		public readonly Guid TransactionID;

		internal TransactionHelper(string connectionString) : this(connectionString, null) { }

		internal TransactionHelper(string connectionString, SqlTransaction txn) : this(connectionString, txn, Guid.NewGuid()) { }

		///<exclude />
		public TransactionHelper(string connectionString, SqlTransaction txn, Guid transactionID)
		{
			this.ConnectionString = connectionString;
			this.Transaction = txn;
			this.TransactionID = transactionID;
		}

		///<exclude />
		public static bool operator==(TransactionHelper a, TransactionHelper b)
		{
			if (object.ReferenceEquals(a, b))
				return true;
			if ((object)a == null || (object)b == null)
				return false;
			return a.CompareTo(b) == 0;
		}

		///<exclude />
		public static bool operator!=(TransactionHelper a, TransactionHelper b)
		{
			if (object.ReferenceEquals(a, b))
				return false;
			if ((object)a == null || (object)b == null)
				return true;
			return a.CompareTo(b) != 0;
		}

		///<exclude />
		public static bool operator<(TransactionHelper a, TransactionHelper b)
		{
			if (object.ReferenceEquals(a, b))
				return false;
			if ((object)a == null || (object)b == null)
				return (object)a == null;
			return a.CompareTo(b) < 0;
		}

		///<exclude />
		public static bool operator>(TransactionHelper a, TransactionHelper b)
		{
			if (object.ReferenceEquals(a, b))
				return false;
			if ((object)a == null || (object)b == null)
				return (object)b == null;
			return a.CompareTo(b) > 0;
		}

		///<exclude />
		public override int GetHashCode()
		{
			return this.TransactionID.GetHashCode();
		}

		///<exclude />
		public override bool Equals(object obj)
		{
			TransactionHelper other = obj as TransactionHelper;
			if (other == null)
				return false;
			return (this.Transaction == null) == (other.Transaction == null) && this.TransactionID == other.TransactionID;
		}

		///<exclude />
		public bool Equals(TransactionHelper other)
		{
			if (other == null)
				return false;
			return (this.Transaction == null) == (other.Transaction == null) && this.TransactionID == other.TransactionID;
		}

		///<exclude />
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			TransactionHelper other = obj as TransactionHelper;
			if (other == null)
				throw new ArgumentException(string.Empty, "obj");
			if ((this.Transaction == null) == (other.Transaction == null))
				return this.TransactionID.CompareTo(other.TransactionID);
			return this.Transaction != null ? 1 : -1;
		}

		///<exclude />
		public int CompareTo(TransactionHelper other)
		{
			if (other == null)
				return 1;
			if ((this.Transaction == null) == (other.Transaction == null))
				return this.TransactionID.CompareTo(other.TransactionID);
			return this.Transaction != null ? 1 : -1;
		}
	}

	/// <summary>
	/// The SqlHelper class is intended to encapsulate high performance, scalable best practices for 
	/// common uses of SqlClient.
	/// </summary>
	public static class SqlHelper
	{
		/// <summary>
		/// The ammount of time given when trying to connect to a database
		/// </summary>
		public static int ConnectionTimeout = -1; // not used yet
		/// <summary>
		/// The ammount of time give when trying to run a SQL command
		/// </summary>
		public static int CommandTimeout = -1;

		private static object _lockObj = new object();
		private static int _lastTick;

		#region private utility methods & constructors

		/// <summary>
		/// This method is used to attach array of SqlParameters to a SqlCommand.
		/// 
		/// This method will assign a value of DbNull to any parameter with a direction of
		/// InputOutput and a value of null.  
		/// 
		/// This behavior will prevent default values from being used, but
		/// this will be the less common case than an intended pure output parameter (derived as InputOutput)
		/// where the user provided no input value.
		/// </summary>
		/// <param name="command">The command to which the parameters will be added</param>
		/// <param name="commandParameters">an array of SqlParameters tho be added to command</param>
		private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
		{
			if (commandParameters == null)
				return;

			foreach (SqlParameter p in commandParameters)
			{
				//check for derived output value with no value assigned
				if (p.Value == null && p.Direction == ParameterDirection.InputOutput)
					p.Value = DBNull.Value;
				command.Parameters.Add(p);
			}
		}

		/// <summary>
		/// This method assigns an array of values to an array of SqlParameters.
		/// </summary>
		/// <param name="commandParameters">array of SqlParameters to be assigned values</param>
		/// <param name="parameterValues">array of objects holding the values to be assigned</param>
		private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
		{
			if (commandParameters == null || parameterValues == null)
				return; //do nothing if we get no data

			// we must have the same number of values as we pave parameters to put them in
			if (commandParameters.Length != parameterValues.Length)
				throw new ArgumentException("Parameter count does not match Parameter Value count.");

			if (commandParameters.Length == 0)
				return;

			//iterate through the SqlParameters, assigning the values from the corresponding position in the value array
			for (int i = 0; i < commandParameters.Length; ++i)
				commandParameters[i].Value = parameterValues[i];
		}

		/// <summary>
		/// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
		/// to the provided command.
		/// </summary>
		/// <param name="command">the SqlCommand to be prepared</param>
		/// <param name="connection">a valid SqlConnection, on which to execute this command</param>
		/// <param name="transaction">a valid SqlTransaction, or 'null'</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
		private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//if the provided connection is not open, we will open it
			if (connection.State != ConnectionState.Open)
				connection.Open();

			//associate the connection with the command
			command.Connection = connection;

			//set the command text (stored procedure name or SQL statement)
			command.CommandText = commandText;

			//if we were provided a transaction, assign it.
			if (transaction != null)
				command.Transaction = transaction;

			//set the command type
			command.CommandType = commandType;

			//attach the command parameters if they are provided
			if (commandParameters != null)
				AttachParameters(command, commandParameters);

			// set the timeout
			if (CommandTimeout >= 0)
				command.CommandTimeout = CommandTimeout;

			return;
		}

		#endregion

		#region Debug Methods

		[Conditional("DEBUG")]
		private static void LogCall()
		{
			int tick = Environment.TickCount;
			Debug.WriteLine(string.Format("Time: {0}  Tics: {1}  Diff: {2}s", DateTime.Now.ToLongTimeString(), tick.ToString(), (((double)(tick - _lastTick)) / 1000d).ToString()));
			_lastTick = tick;
		}

		[Conditional("DEBUG")]
		private static void LogCall(CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			int tick = Environment.TickCount;
			Debug.WriteLine(string.Format("Time: {0}  Tics: {1}  Type: {2}  Command: {3}", DateTime.Now.ToLongTimeString(), tick.ToString(), commandType.ToString(), commandText));
			if (commandParameters != null)
				foreach (SqlParameter param in commandParameters)
					Debug.WriteLine(string.Format("Parameter[{0}]={1}", param.ParameterName, param.Value));
			_lastTick = tick;
		}

		#endregion

		#region Transaction Processing

		/// <summary>
		/// Starts a transaction on the database specified in the connection string
		/// </summary>
		/// <param name="connectionString">The connection string for the database on which the transaction will be started</param>
		/// <param name="txnh">The TransactionHelper object representing the transaction</param>
		/// <returns>A boolean that indicates the success result of starting the transaction</returns>
		public static bool BeginTransaction(string connectionString, out TransactionHelper txnh)
		{
			try
			{
				// Start a local transaction
				SqlConnection conn = new SqlConnection(connectionString);
				conn.Open();
				try
				{
					txnh = new TransactionHelper(connectionString, conn.BeginTransaction(IsolationLevel.ReadUncommitted));
				}
				catch
				{
					conn.Close();
					throw;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
				txnh = null;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Commits a transaction
		/// </summary>
		/// <param name="txnh">The transaction that will be committed</param>
		/// <returns>A boolean that indicates the success result of committing the transaction</returns>
		public static bool CommitTransaction(TransactionHelper txnh)
		{
			if (txnh == null)
				return false;

			try
			{
				// Commit local transaction
				SqlConnection conn = txnh.Transaction.Connection;
				try
				{
					txnh.Transaction.Commit();
				}
				catch
				{
					try
					{
						txnh.Transaction.Rollback();
					}
					catch { }
					throw;
				}
				finally
				{
					if (conn != null && conn.State != ConnectionState.Closed)
						conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Rolls back a transaction
		/// </summary>
		/// <param name="txnh">The transaction that will be rolled back</param>
		/// <returns>A boolean that indicates the success result of rolling back the transaction</returns>
		public static bool RollbackTransaction(TransactionHelper txnh)
		{
			if (txnh == null)
				return false;

			try
			{
				// roll back local transaction
				SqlConnection conn = txnh.Transaction.Connection;
				try
				{
					txnh.Transaction.Rollback();
				}
				finally
				{
					if (conn != null && conn.State != ConnectionState.Closed)
						conn.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
				return false;
			}
		}

		#endregion

		#region ExecuteNonQuery

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connectionString, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, "PublishOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored prcedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteNonQuery(connectionString, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, "PublishOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored prcedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connectionString, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(connection, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

				LogCall(commandType, commandText, commandParameters);

				//finally, execute the command.
				int retval = cmd.ExecuteNonQuery();

				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();

				LogCall();

				return retval;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, "PublishOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteNonQuery(connection, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, "PublishOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection connection, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset and takes no parameters) against the provided transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TransactionHelper txnh, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteNonQuery(txnh, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the specified transaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			if (txnh == null)
				throw new ArgumentNullException("txnh");

			LogCall(commandType, commandText, commandParameters);

			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);

				//finally, execute the command.
				int retval = cmd.ExecuteNonQuery();

				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();

				LogCall();

				return retval;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, trans, "PublishOrders");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TransactionHelper txnh, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteNonQuery(txnh, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified 
		/// transaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(TransactionHelper txnh, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteNonQuery(txnh, CommandType.StoredProcedure, spName); // just call the SP without params

			if (txnh == null)
				throw new ArgumentNullException("txnh");

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteNonQuery(txnh, CommandType.StoredProcedure, spName, commandParameters);
		}

		#endregion ExecuteNonQuery

		#region ExecuteDataSet

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connectionString, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteDataset(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteDataset(connectionString, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(connString, "GetOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(string connectionString, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(connection, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			LogCall(commandType, commandText, commandParameters);

			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

				//create the DataAdapter & DataSet
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();

					//fill the DataSet using default values for DataTable names, etc.
					da.Fill(ds);

					// detach the SqlParameters from the command object, so they can be used again.			
					cmd.Parameters.Clear();

					LogCall();

					//return the dataset
					return ds;
				}
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, "GetOrders");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteDataset(connection, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(conn, "GetOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(SqlConnection connection, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteDataset(connection, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset and takes no parameters) against the provided transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TransactionHelper txnh, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteDataset(txnh, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a resultset) against the specified transaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			if (txnh == null)
				throw new ArgumentNullException("txnh");

			LogCall(commandType, commandText, commandParameters);

			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);

				//create the DataAdapter & DataSet
				using (SqlDataAdapter da = new SqlDataAdapter(cmd))
				{
					DataSet ds = new DataSet();

					//fill the DataSet using default values for DataTable names, etc.
					da.Fill(ds);

					// detach the SqlParameters from the command object, so they can be used again.
					cmd.Parameters.Clear();

					LogCall();

					//return the dataset
					return ds;
				}
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, "GetOrders");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TransactionHelper txnh, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteDataset(txnh, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified 
		/// transaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  DataSet ds = ExecuteDataset(trans, "GetOrders", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>a dataset containing the resultset generated by the command</returns>
		public static DataSet ExecuteDataset(TransactionHelper txnh, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteDataset(txnh, CommandType.StoredProcedure, spName); // just call the SP without params

			if (txnh == null)
				throw new ArgumentNullException("txnh");

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteDataset(txnh, CommandType.StoredProcedure, spName, commandParameters);
		}

		#endregion ExecuteDataSet

		#region ExecuteScalar

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
		/// the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connectionString, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			//create & open a SqlConnection, and dispose of it after we are done.
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();

				//call the overload that takes a connection in place of the connection string
				return ExecuteScalar(cn, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in the connection string. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount");
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteScalar(connectionString, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in 
		/// the connection string using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(string connectionString, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(connection, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			LogCall(commandType, commandText, commandParameters);

			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

				//execute the command & return the results
				object retval = cmd.ExecuteScalar();

				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();

				LogCall();

				return retval;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount");
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteScalar(connection, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection 
		/// using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="connection">a valid SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(SqlConnection connection, string spName, object[] parameterValues)
		{
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteScalar(connection, CommandType.StoredProcedure, spName); // just call the SP without params

			//if we receive parameter values, we need to figure out where they go
			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset and takes no parameters) against the provided transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TransactionHelper txnh, CommandType commandType, string commandText)
		{
			//pass through the call providing null for the set of SqlParameters
			return ExecuteScalar(txnh, commandType, commandText, null);
		}

		/// <summary>
		/// Execute a SqlCommand (that returns a 1x1 resultset) against the specified transaction
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter[] { new SqlParameter("@prodid", 24) });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TransactionHelper txnh, CommandType commandType, string commandText, SqlParameter[] commandParameters)
		{
			if (txnh == null)
				throw new ArgumentNullException("txnh");

			LogCall(commandType, commandText, commandParameters);

			//create a command and prepare it for execution
			using (SqlCommand cmd = new SqlCommand())
			{
				PrepareCommand(cmd, txnh.Transaction.Connection, txnh.Transaction, commandType, commandText, commandParameters);

				//execute the command & return the results
				object retval = cmd.ExecuteScalar();

				// detach the SqlParameters from the command object, so they can be used again.
				cmd.Parameters.Clear();

				LogCall();

				return retval;
			}
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified transaction. 
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount");
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TransactionHelper txnh, string spName)
		{
			//pass through the call providing null for the set of objects
			return ExecuteScalar(txnh, spName, null);
		}

		/// <summary>
		/// Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified
		/// transaction using the provided parameter values.  This method will query the database to discover the parameters for the 
		/// stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.
		/// </summary>
		/// <remarks>
		/// This method provides no access to output parameters or the stored procedure's return value parameter.
		/// 
		/// e.g.:  
		///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", new object[] { 24, 36 });
		/// </remarks>
		/// <param name="txnh">a valid TransactionHelper</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="parameterValues">an array of objects to be assigned as the input values of the stored procedure</param>
		/// <returns>an object containing the value in the 1x1 resultset generated by the command</returns>
		public static object ExecuteScalar(TransactionHelper txnh, string spName, object[] parameterValues)
		{
			//if we receive parameter values, we need to figure out where they go
			if (parameterValues == null || parameterValues.Length == 0)
				return ExecuteScalar(txnh, CommandType.StoredProcedure, spName); // just call the SP without params

			if (txnh == null)
				throw new ArgumentNullException("txnh");

			//pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
			SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(txnh.Transaction.Connection.ConnectionString, spName);

			//assign the provided values to these parameters based on parameter order
			AssignParameterValues(commandParameters, parameterValues);

			//call the overload that takes an array of SqlParameters
			return ExecuteScalar(txnh, CommandType.StoredProcedure, spName, commandParameters);
		}

		#endregion ExecuteScalar
	}

	/// <summary>
	/// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public static class SqlHelperParameterCache
	{
		#region private methods, variables, and constructors

		private static Dictionary<string, SqlParameter[]> _paramCache = new Dictionary<string, SqlParameter[]>();
		private static object _lockObj = new object();

		/// <summary>
		/// resolve at run time the appropriate set of SqlParameters for a stored procedure
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
		{
			using (SqlConnection cn = new SqlConnection(connectionString))
			{
				cn.Open();
				using (SqlCommand cmd = new SqlCommand(spName, cn))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					SqlCommandBuilder.DeriveParameters(cmd);
					if (!includeReturnValueParameter)
						cmd.Parameters.RemoveAt(0);

					SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;
					cmd.Parameters.CopyTo(discoveredParameters, 0);
					return discoveredParameters;
				}
			}
		}

		//deep copy of cached SqlParameter array
		private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
		{
			SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

			for (int i = 0; i < originalParameters.Length; ++i)
				clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters to be cached</param>
		public static void CacheParameterSet(string connectionString, string commandText, SqlParameter[] commandParameters)
		{
			lock (_lockObj)
			{
				_paramCache[string.Format("{0}:{1}", connectionString, commandText)] = commandParameters;
			}
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <returns>an array of SqlParamters</returns>
		public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
		{
			string hashKey = string.Format("{0}:{1}", connectionString, commandText);
			SqlParameter[] cachedParameters;
			lock (_lockObj)
			{
				_paramCache.TryGetValue(hashKey, out cachedParameters);
			}
			return cachedParameters != null ? CloneParameters(cachedParameters) : null;
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
		{
			return GetSpParameterSet(connectionString, spName, false);
		}

		/// <summary>
		/// Retrieves the set of SqlParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of SqlParameters</returns>
		public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
		{
			string hashKey = string.Format(!includeReturnValueParameter ? "{0}:{1}" : "{0}:{1}:include ReturnValue Parameter", connectionString, spName);
			SqlParameter[] cachedParameters;
			lock (_lockObj)
			{
				_paramCache.TryGetValue(hashKey, out cachedParameters);
			}

			if (cachedParameters == null)
			{
				cachedParameters = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter);
				lock (_lockObj)
				{
					_paramCache[hashKey] = cachedParameters;
				}
			}

			return cachedParameters != null ? CloneParameters(cachedParameters) : null;
		}

		#endregion Parameter Discovery Functions
	}
	
	
}
