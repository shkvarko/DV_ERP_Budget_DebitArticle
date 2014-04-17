using System;
using System.Collections.Generic;
using System.Text;

namespace DebitArticle
{
    public class CDebitArticleModuleClassInfo : UniXP.Common.CModuleClassInfo
    {
        public CDebitArticleModuleClassInfo()
        {
            // статьи расходов
            UniXP.Common.CLASSINFO objClassInfo = new UniXP.Common.CLASSINFO() { enClassType = UniXP.Common.EnumClassType.mcView,
                strClassName = "DebitArticle.ViewDebitArticle",
                strName = "Справочник статей расходов",
                strDescription = "Модуль для редактирования списка статей расходов",
                lID = 0,
                nImage = 1,
                strResourceName = "IMAGES_DEBITARTICLESMALL" };
            
            m_arClassInfo.Add( objClassInfo );
            
            // бюджетные подразделения
            objClassInfo = new UniXP.Common.CLASSINFO() { enClassType = UniXP.Common.EnumClassType.mcView, 
                strClassName = "DebitArticle.ViewBudgetDep", 
                strName = "Справочник бюджетных подразделений", 
                strDescription = "Модуль для редактирования списка бюджетных подразделений", 
                lID = 1, 
                nImage = 1, 
                strResourceName = "IMAGES_BUDGETDEPSMALL" };
            
            m_arClassInfo.Add( objClassInfo );
        }
    }

    public class CDebitArticleModuleInfo : UniXP.Common.CClientModuleInfo
    {
        public CDebitArticleModuleInfo()
            : base( System.Reflection.Assembly.GetExecutingAssembly(),
            UniXP.Common.EnumDLLType.typeItem,
            new System.Guid( "{E0F56B9B-AF69-4427-B6F1-8144393596F3}" ),
            new System.Guid( "{a8e694df-15a3-4713-80ac-304b3fe911e8}" ),
            DebitArticle.Properties.Resources.IMAGES_DEBITARTICLE,
            DebitArticle.Properties.Resources.IMAGES_DEBITARTICLESMALL )
        {
        }

        /// <summary>
        /// Выполняет операции по проверке правильности установки модуля в системе.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Check( UniXP.Common.CProfile objProfile )
        {
            return true;
        }
        /// <summary>
        /// Выполняет операции по установке модуля в систему.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Install( UniXP.Common.CProfile objProfile )
        {
            return true;
        }
        /// <summary>
        /// Выполняет операции по удалению модуля из системы.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean UnInstall( UniXP.Common.CProfile objProfile )
        {
            return true;
        }
        /// <summary>
        /// Производит действия по обновлению при установке новой версии подключаемого модуля.
        /// </summary>
        /// <param name="objProfile">Профиль пользователя.</param>
        public override System.Boolean Update( UniXP.Common.CProfile objProfile )
        {
            return true;
        }
        /// <summary>
        /// Возвращает список доступных классов в данном модуле.
        /// </summary>
        public override UniXP.Common.CModuleClassInfo GetClassInfo()
        {
            return new CDebitArticleModuleClassInfo();
        }
    }

    public class ModuleInfo : PlugIn.IModuleInfo
    {
        public UniXP.Common.CClientModuleInfo GetModuleInfo()
        {
            return new CDebitArticleModuleInfo();
        }
    }

}
