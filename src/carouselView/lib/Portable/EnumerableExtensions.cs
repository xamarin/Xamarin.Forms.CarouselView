using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarin.Forms
{
	internal static class EnumerableExtensions
	{
		internal static IReadOnlyList<object> ToReadOnlyList(this IEnumerable enumerable)
		{
			var readOnlyList = enumerable as IReadOnlyList<object>;
			if(readOnlyList != null)
				return readOnlyList;

			var list = enumerable as IList;
			if(list != null)
				return new ListAsReadOnlyList(list);

			var objectList = enumerable as IList<object>;
			if(objectList != null)
				return new GenericListAsReadOnlyList<object>(objectList);

			// allow IList<AnyType> without falling through to the array copy below
			var typedList = (IReadOnlyList<object>)(
				from iface in enumerable.GetType().GetTypeInfo().ImplementedInterfaces
				where iface.Name == typeof(IList<>).Name && iface.GetGenericTypeDefinition() == typeof(IList<>)
				let type = typeof(GenericListAsReadOnlyList<>).MakeGenericType(iface.GenericTypeArguments[0])
				select Activator.CreateInstance(type, enumerable)
			).FirstOrDefault();
			if(typedList != null)
				return typedList;

			// ToArray instead of ToList to save memory
			return enumerable.Cast<object>().ToArray();
		}

		class ListAsReadOnlyList : IReadOnlyList<object>
		{
			IList _list;

			internal ListAsReadOnlyList(IList list)
			{
				_list = list;
			}

			public object this[int index] => _list[index];
			public int Count => _list.Count;
			public IEnumerator<object> GetEnumerator() => _list.Cast<object>().GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		class GenericListAsReadOnlyList<T> : IReadOnlyList<object>
		{
			IList<T> _list;

			public GenericListAsReadOnlyList(IList<T> list)
			{
				_list = list;
			}

			public object this[int index] => _list[index];
			public int Count => _list.Count;
			public IEnumerator<object> GetEnumerator() => _list.Cast<object>().GetEnumerator();
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
