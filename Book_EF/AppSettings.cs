using System;
using System.Collections.Generic;
using System.Text;

namespace Book_EF
{
  public class AppSettings
  {
    private static AppSettings instance;

    /// <summary>
    /// インスタンス
    /// </summary>
    public static AppSettings I { get { return instance; } }

    public string ConnectionString { get; set; }

    private AppSettings() { }

    public static void Init(string connectionString)
    {
      instance = new AppSettings();
      instance.ConnectionString = connectionString;
    }
  }
}
