using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrocCSarpBot
{


    /// <summary>
    /// Главный класс приложения
    /// </summary>
    class Program
    {

        /// <summary>
        /// Ведение журнала событий
        /// </summary>
        private static NLog.Logger _log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Параметры командной строки</param>
        static void Main(string[] args)
        {
            try
            {
                var bot = new Bot();
                bot.Run();
                _log.Info("Запуск бота в консольном режиме.");
            }
            catch (Exception e)
            {
                // Отображание исключения, включая все вложенные исключения
                do
                {
                    Console.WriteLine(e.Message);
                    e = e.InnerException;
                } while (e != null);
                
            }
            finally
            {
                _log.Info("Нажмите Enter для завершения.");
                Console.ReadLine();
            }

        }
    }
}
