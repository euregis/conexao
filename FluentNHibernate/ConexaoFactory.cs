using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;

namespace Conexao.FluentNHibernate
{
    public abstract class ConexaoFactory
    {
        public abstract IPersistenceConfigurer GetConexao(string[] param);
    }
}
