﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI.Extensions;

namespace Wolfje.Plugins.SEconomy.Extensions {
	public static class MySqlConnectionExtensions {

		/// <summary>
		/// Executes a query on a database and sets identity to the last inserted identity in that table.
		/// </summary>
		/// <param name="olddb">Database to query</param>
		/// <param name="query">Query string with parameters as @0, @1, etc.</param>
		/// <param name="args">Parameters to be put in the query</param>
		/// <returns>Rows affected by query</returns>
		[SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static int QueryIdentity(this MySql.Data.MySqlClient.MySqlConnection olddb, string query, out long identity, params object[] args)
		{
			using (var db = new MySql.Data.MySqlClient.MySqlConnection(olddb.ConnectionString)) {
				db.Open();

				using (var com = db.CreateCommand()) {
					com.CommandText = query;
					for (int i = 0; i < args.Length; i++)
						com.AddParameter("@" + i, args[i]);

					
					int affected = com.ExecuteNonQuery();
					identity = com.LastInsertedId;
					return affected;
				}
			}
		}

		/// <summary>
		/// Executes a query on a database.
		/// </summary>
		/// <param name="olddb">Database to query</param>
		/// <param name="query">Query string with parameters as @0, @1, etc.</param>
		/// <param name="args">Parameters to be put in the query</param>
		/// <returns>Query result as IDataReader</returns>
		[SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public static T QueryScalar<T>(this MySql.Data.MySqlClient.MySqlConnection olddb, string query, params object[] args)
		{
			object result = null;

			using (var db = new MySql.Data.MySqlClient.MySqlConnection(olddb.ConnectionString)) {
				db.Open();

				using (var com = db.CreateCommand()) {
					com.CommandText = query;
					
					for (int i = 0; i < args.Length; i++) {
						com.AddParameter("@" + i, args[i]);
					}

					if ((result = com.ExecuteScalar()) == null) {
						return default(T);
					}

					return (T)result;
				}
			}
		}
	}
}
