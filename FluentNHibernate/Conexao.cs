using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Conexao.FluentNHibernate
{
    public enum TipoConexao {SQLite, SQLServer, Indefinido}
    public class Conexao
    {
        public ISession Sessao {get;set;}
        private ISessionFactory sessionFactory = null;
        public List<Assembly> Assemblys { get; set; }
        public IPersistenceConfigurer DataBase;
        public Configuration Configuracao;

        public Conexao(TipoConexao tipoConexao, string[] param, List<Assembly> mapeamentos)
        {
            if(mapeamentos!=null)
                Assemblys = mapeamentos;
			else
			    Assemblys = new List<Assembly>();
            ConfigDataBase(tipoConexao, param);
		}
		private void ConfigDataBase(TipoConexao tipoConexao, string[] param)
		{
			ConexaoFactory conexaoFactory;
			if(tipoConexao == TipoConexao.SQLite)
				conexaoFactory = new SQLiteConexaoFactory();
			else
				conexaoFactory = null;
			
			if(conexaoFactory != null)
				DataBase= conexaoFactory.GetConexao(param);
		}
		
		public void AbrirSessao()
		{
            if (Sessao == null || !Sessao.IsOpen)
            {
                if (sessionFactory == null)
                    sessionFactory = CreateSessionFactory();
                Sessao = sessionFactory.OpenSession();
            }
          
		}
		
		public void FecharSessao()
		{
			if(Sessao != null && Sessao.IsOpen)
			{
				Sessao.Close();
			}
		}
		
		private ISessionFactory CreateSessionFactory(){
			FluentConfiguration fConfig;
			try{
				fConfig = Fluently.Configure().Database(DataBase);
					foreach(Assembly item in Assemblys)
						fConfig.Mappings(m=> m.FluentMappings.AddFromAssembly(item));
					fConfig.ExposeConfiguration(BuildSchema);
						return 				fConfig.BuildSessionFactory();
				}
				catch(Exception)
				{
					return null;
				}
			}


        private void BuildSchema(Configuration config)
        {
            //new SchemaExport(config).Create(true, true);
            new SchemaUpdate(config).Execute(true, true);
            Configuracao = config;
        }
    }
}
