namespace ETS.Data.ConnString.Common
{
	/// <summary>
	/// ConnectionString definition class.
	/// </summary>
	public interface IConnectionString
	{
		/// <summary>
		/// get or set the connString name which uniquely identifies a connString
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// get or set the database connectionString.
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// get or set the database providerName.
		/// </summary>
		string ProviderName { get; set; }
	}
}