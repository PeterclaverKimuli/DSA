using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public abstract class Entry
    {
        public Directory Parent {  get; protected set; }
        public string Name {  get; protected set; }
        public DateTime CreatedDate {  get; protected set; }
        public DateTime LastUpdatedDate {  get; protected set; }

        public Entry(Directory parent, string name)
        {
            Parent = parent;
            Name = name;
            CreatedDate = DateTime.Now;
            LastUpdatedDate = DateTime.Now;
        }

        public abstract int Size();

        public string GetFullPath()
        {
            if (Parent == null) return Name;
            else return Parent.GetFullPath() + "/" + Name;
        }

        public bool Delete()
        {
            if(Parent == null) return false;

            return Parent.RemoveEntry(this);
        }

        public void ChangeName(string name) { Name = name; }
    }

    public class FileClass : Entry
    {
        private int _size;

        public string Content { get; set; }

        public FileClass(Directory parent, string name, int size) : base(parent, name)
        {
            _size = size;
        }

        public override int Size() { return _size; }
    }

    public class Directory : Entry
    {
        public List<Entry> contents { protected get; set; }

        public Directory(string name, Directory parent) : base(parent, name)
        {
            contents = new List<Entry>();
        }

        public override int Size()
        {
            int size = 0;

            foreach(Entry entry in contents)
            {
                size += entry.Size();
            }

            return size;
        }

        public int NumberOfFiles()
        {
            int count = 0;

            foreach(Entry entry in contents)
            {
                if(entry is Directory d)
                {
                    count++;

                    var dir = d as Directory;

                    count += dir.NumberOfFiles();
                }
                else if(entry is FileClass)
                    count++;
            }

            return count;
        }

        public bool RemoveEntry(Entry entry)
        {
            return contents.Remove(entry);
        }
    }
}
