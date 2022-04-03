using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Shedule
{
    public static class Message
    {
        public static void Error_NonExistSqlLocalDb()
        {
            MessageBox.Show(
                "Отсутствует установленная программа Microsoft SQL Server LocalDB!\nУстановите программу или обратитесь к системному администратору.",
                "Ошибка!",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        /// <summary>
        /// Вызывает всплывающее окно с подтвержением действия на удаление записи и возвращает результат подтвержения.
        /// </summary>
        /// <returns></returns>
        public static MessageBoxResult Action_DeleteRecord(int countRecord)
        {
            return MessageBox.Show(
                "Вы действительно хотите удалить записи?",
                $"Удаление записи ({countRecord})",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning,
                MessageBoxResult.Yes);
        }

        public static MessageBoxResult Action_DeleteDatabase()
        {
            return MessageBox.Show(
                "Вы действительно хотите удалить все данные?\nОтменить данное действие будет невозможно.",
                "Удаление данных",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        public static void Error_UnknownAdd()
        {
            MessageBox.Show(
                "Запись не добавлена! Обратитесь к системному администратору.",
                "Ошибка!",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void Error_UnknownChange()
        {
            MessageBox.Show(
                "Запись не изменена! Обратитесь к системному администратору.",
                "Ошибка!",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void Information_RecordWithoutChange()
        {
            MessageBox.Show(
                "В записи нету изменений. Внесите измения в запись или закройте окно.",
                "Уведомление",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public static void Warning_EmptyFields()
        {
            MessageBox.Show(
                "Не все поля заполнены!",
                "Ошибка!",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}
