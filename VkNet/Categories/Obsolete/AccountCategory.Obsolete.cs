﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories
{
	/// <summary>
	/// Методы этого класса позволяют производить действия с аккаунтом пользователя.
	/// </summary>
	public partial class AccountCategory
	{
		/// <summary>
		/// Подписывает устройство на базе iOS, Android или Windows Phone на получение Push-уведомлений.
		/// </summary>
		/// <param name="token">Идентификатор устройства, используемый для отправки уведомлений. (для mpns идентификатор должен представлять из себя URL для отправки уведомлений)</param>
		/// <param name="deviceModel">Строковое название модели устройства.</param>
		/// <param name="systemVersion">Строковая версия операционной системы устройства.</param>
		/// <param name="noText">Не передавать текст сообщения в push уведомлении. (по умолчанию текст передается)</param>
		/// <param name="subscribe">Список типов уведомлений, которые следует присылать. По умолчанию присылаются: SubscribeFilter.Message</param>
		/// <returns>Возвращает результат выполнения метода.</returns>
		/// <remarks>
		/// Страница документации ВКонтакте https://vk.com/dev/account.registerDevice
		/// </remarks>
		[Obsolete("Функция устарела. Пожалуйста используйте функцию RegisterDevice(AccountRegisterDeviceParams @params)")]
		public bool RegisterDevice([NotNull]string token, string deviceModel, string systemVersion, bool? noText = null, SubscribeFilter subscribe = null)
		{
			VkErrors.ThrowIfNullOrEmpty(() => token);

			var parameters = new AccountRegisterDeviceParams
			{
				Token = token,
				DeviceModel = deviceModel,
				SystemVersion = systemVersion
			};

			return RegisterDevice(parameters);
		}

		/// <summary>
		///  Редактирует информацию текущего профиля.
		/// </summary>
		/// <param name="firstName">Имя пользователя</param>
		/// <param name="lastName">Фамилия пользователя</param>
		/// <param name="maidenName">Девичья фамилия пользователя</param>
		/// <param name="sex">Пол пользователя</param>
		/// <param name="relation">Семейное положение пользователя</param>
		/// <param name="relationPartnerId">Идентификатор пользователя, с которым связано семейное положение</param>
		/// <param name="birthDate">Дата рождения пользователя</param>
		/// <param name="birthDateVisibility">Видимость даты рождения</param>
		/// <param name="homeTown">Родной город пользователя</param>
		/// <param name="countryId">Идентификатор страны пользователя</param>
		/// <param name="cityId">Идентификатор города пользователя</param>
		/// <returns>Результат выполнения операции.</returns>
		/// <remarks> Если передаются <paramref name="firstName"/> или <paramref name="lastName"/>, рекомендуется
		/// использовать перегрузку с соотвествующим out параметром типа ChangeNameRequest</remarks>
		[Obsolete("Данный метод устарел, пожалуйста используйте метод SaveProfileInfo(out ChangeNameRequest changeNameRequest, AccountSaveInfo @params)")]
		public bool SaveProfileInfo(string firstName = null, string lastName = null, string maidenName = null, Sex? sex = null,
			RelationType? relation = null, long? relationPartnerId = null, DateTime? birthDate = null, BirthdayVisibility? birthDateVisibility = null,
			string homeTown = null, long? countryId = null, long? cityId = null)
		{
			ChangeNameRequest request;
			var parameters = new AccountSaveProfileInfoParams
			{
				FirstName = firstName,
				LastName = lastName,
				MaidenName = maidenName,
				Sex = sex.Value,
				Relation = relation.Value,
				RelationPartner = relationPartnerId.HasValue ? new User { Id = relationPartnerId.Value } : null,
				BirthDate = birthDate?.ToShortDateString(),
				BirthdayVisibility = birthDateVisibility.Value,
				HomeTown = homeTown,
				Country = new Country { Id = countryId },
				City = new City
				{
					Id = cityId
				}
			};
			return SaveProfileInfo(out request, parameters);
		}

		/// <summary>
		///  Редактирует информацию текущего профиля.
		/// </summary>
		/// <param name="changeNameRequest">Если в параметрах передавалось имя или фамилия пользователя,
		/// в этом параметре будет возвращен объект типа ChangeNameRequest</param>
		/// <param name="firstName">Имя пользователя</param>
		/// <param name="lastName">Фамилия пользователя</param>
		/// <param name="maidenName">Девичья фамилия пользователя</param>
		/// <param name="sex">Пол пользователя</param>
		/// <param name="relation">Семейное положение пользователя</param>
		/// <param name="relationPartnerId">Идентификатор пользователя, с которым связано семейное положение</param>
		/// <param name="birthDate">Дата рождения пользователя</param>
		/// <param name="birthDateVisibility">Видимость даты рождения</param>
		/// <param name="homeTown">Родной город пользователя</param>
		/// <param name="countryId">Идентификатор страны пользователя</param>
		/// <param name="cityId">Идентификатор города пользователя</param>
		/// <returns>Результат выполнения операции.</returns>
		[Obsolete("Данный метод устарел, пожалуйста используйте метод SaveProfileInfo(out ChangeNameRequest changeNameRequest, AccountSaveInfo @params)")]
		public bool SaveProfileInfo(out ChangeNameRequest changeNameRequest, string firstName = null, string lastName = null, string maidenName = null, Sex? sex = null,
			RelationType? relation = null, long? relationPartnerId = null, DateTime? birthDate = null, BirthdayVisibility? birthDateVisibility = null,
			string homeTown = null, long? countryId = null, long? cityId = null)
		{
			var parameters = new AccountSaveProfileInfoParams
			{
				FirstName = firstName,
				LastName = lastName,
				MaidenName = maidenName,
				Sex = sex.Value,
				Relation = relation.Value,
				RelationPartner = relationPartnerId.HasValue ? new User
				{
					Id = relationPartnerId.Value
				} : null,
				BirthDate = birthDate?.ToShortDateString(),
				BirthdayVisibility = birthDateVisibility.Value,
				HomeTown = homeTown,
				Country = new Country { Id = countryId },
				City = new City { Id = cityId }
			};

			return SaveProfileInfo(out changeNameRequest, parameters);
		}

		/// <summary>
		/// Возвращает список пользователей, находящихся в черном списке.
		/// </summary>
		/// <param name="total">Возвращает общее количество находящихся в черном списке пользователей.</param>
		/// <param name="offset">Смещение необходимое для выборки определенного подмножества черного списка. положительное число (Положительное число).</param>
		/// <param name="count">Количество записей, которое необходимо вернуть. положительное число, по умолчанию 20, максимальное значение 200 (Положительное число, по умолчанию 20, максимальное значение 200).</param>
		/// <returns>
		/// Возвращает набор объектов пользователей, находящихся в черном списке.
		/// </returns>
		/// <remarks>
		/// Страница документации ВКонтакте http://vk.com/dev/account.getBanned
		/// </remarks>
		[Obsolete("Метод устарел, пожалуйста используйте метод GetBanned(int? offset = null, int? count = null)")]
		public ReadOnlyCollection<User> GetBanned(out int total, int? offset = null, int? count = null)
		{
			var response = GetBanned(offset, count);

			total = Convert.ToInt32(response.TotalCount);

			return response.ToReadOnlyCollection();
		}

        /// <summary>
        /// Позволяет искать пользователей ВКонтакте, используя телефонные номера, email-адреса, и идентификаторы пользователей в других сервисах. Найденные пользователи могут быть также в дальнейшем получены методом friends.getSuggestions.
        /// </summary>
        /// <param name="contacts">Список контактов, разделенных через запятую. список слов, разделенных через запятую (Список слов, разделенных через запятую).</param>
        /// <param name="service">Строковой идентификатор сервиса, по контактам которого производится поиск. Может принимать следующие значения: (email, phone, twitter, facebook, odnoklassniki, instagram, google) строка, обязательный параметр (Строка, обязательный параметр).</param>
        /// <param name="mycontact">Контакт текущего пользователя в заданном сервисе. строка (Строка).</param>
        /// <param name="returnAll">1 – возвращать также контакты, найденные ранее с использованием этого сервиса, 0 – возвращать только контакты, найденные с использованием поля contacts. флаг, может принимать значения 1 или 0 (Флаг, может принимать значения 1 или 0).</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть.
        /// Доступные значения: nickname, domain, sex, bdate, city, country, timezone, photo_50, photo_100, photo_200_orig, has_mobile, contacts, education, online, relation, last_seen, status, can_write_private_message, can_see_all_posts, can_post, universities список слов, разделенных через запятую (Список слов, разделенных через запятую).</param>
        /// <returns>
        /// В качестве результата метод возвращает два списка:
        /// found – список объектов пользователей, расширенных полями contact – контакт, по которому был найден пользователь (не приходит если пользователь был найден при предыдущем использовании метода), request_sent – запрос на добавление в друзья уже был выслан, либо пользователь уже является другом, common_count если этот контакт также был импортирован друзьями или контактами текущего пользователя. Метод также возвращает найденные ранее контакты.
        /// other – список контактов, которые не были найдены. Объект содержит поля contact и common_count если этот контакт также был импортирован друзьями или контактами текущего пользователя.
        /// </returns>
        /// <remarks>
        /// Страница документации ВКонтакте http://vk.com/dev/account.lookupContacts
        /// </remarks>
        [Obsolete("Данный метод устарел и может быть отключён через некоторое время, пожалуйста, избегайте его использования.")]
        public LookupContactsResult LookupContacts(List<string> contacts, Services service, string mycontact = null, bool? returnAll = null, UsersFields fields = null)
        {
            var parameters = new VkParameters
            {
                { "contacts", contacts },
                { "service", service },
                { "mycontact", mycontact },
                { "return_all", returnAll },
                { "fields", fields }
            };
            return _vk.Call("account.lookupContacts", parameters);
        }
	}
}