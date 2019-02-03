using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace Conexao.FluentNHibernate
{
    class SQLiteConexaoFactory:ConexaoFactory
    {
        public override IPersistenceConfigurer GetConexao(string[] param)
        {
            if (param.Length == 1)
                return SQLiteConfiguration.Standard.UsingFile(param[0]);
            else if (param.Length == 2)
                return SQLiteConfiguration.Standard.UsingFileWithPassword(param[0], param[1]);
            else
                return null;
        }
    }
}
