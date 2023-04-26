using System.ComponentModel.DataAnnotations;
using System;

namespace UserAPI.Configuration
{
  public sealed class IncludeArray : ValidationAttribute
  {
    public object[] CheckArray { get; set; }
    private bool allowNull;

    public IncludeArray(bool allowNull = false)
    {
      this.allowNull = allowNull;
    }

    public override bool IsValid(object value)
    {
      if (value == null) return allowNull;
      if (CheckArray == null) return true;
      bool result = false;
      foreach (object item in CheckArray)
        if (value.Equals(item))
        {
          result = true;
          break;
        }
      return result;
    }
  }

  public class Table : Attribute
  {
    public string Name { get; set; }

    public Table(string name)
    {
      this.Name = name;
    }
  }

  public class TableRow : Attribute
  {
    public string Name { get; set; }

    public TableRow(string name)
    {
      this.Name = name;
    }
  }
}
