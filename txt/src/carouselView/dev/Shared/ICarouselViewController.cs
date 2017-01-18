using System.Collections.Specialized;

namespace Xamarin.Forms
{
	public interface ICarouselViewController : IItemViewController
	{
		bool IgnorePositionUpdates
		{
			get;
		}
		void SendSelectedItemChanged(object item);
		void SendSelectedPositionChanged(int position);

		event NotifyCollectionChangedEventHandler CollectionChanged;

		int Position { get; set; }
		object Item { get; set; }        
	}

}
