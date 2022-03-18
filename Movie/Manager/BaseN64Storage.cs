using Microsoft.Toolkit.Mvvm.ComponentModel;
using MupenToolkitPRE.Movie.Definitions;
using MupenToolkitPRE.Movie.Definitions.M64;
using PostSharp.Patterns.Model;
using System.Collections.ObjectModel;
using System.IO;

namespace MupenToolkitPRE.Movie.Manager
{
    [NotifyPropertyChanged]
    public abstract class BaseN64Storage : ObservableObject
    {
        public abstract string Extension { get; }
        public virtual string Name { get; set; }
        public virtual bool Playing { get; set; }

        public abstract InteractionStatus Save(string path, ObservableCollection<ObservableCollection<Sample>> inputs);
        protected abstract (InteractionStatus Status, MovieHeader? Header) LoadHeader(BinaryReader br);
        protected abstract (InteractionStatus Status, ObservableCollection<ObservableCollection<Sample>>? Samples) LoadInputs(BinaryReader br, MovieHeader header);
        public abstract (InteractionStatus Status, ObservableCollection<ObservableCollection<Sample>> Samples) Load(string path);
    }
}
