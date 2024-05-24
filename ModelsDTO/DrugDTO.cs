using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hospital.Models;

public class DrugDTO : INotifyPropertyChanged
{
    public int Id { get; set; }


    private string _name = null!;

    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }

    private Manufacturer _manufacturer = null!;

    public Manufacturer Manufacturer
    {
        get { return _manufacturer; }
        set
        {
            _manufacturer = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Manufacturer)));
        }
    }

    private DrugProvider _drugProvider = null!;

    public DrugProvider DrugProvider
    {
        get { return _drugProvider; }
        set
        {
            _drugProvider = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DrugProvider)));
        }
    }

    private int _count;

    public int Count
    {
        get { return _count; }
        set
        {
            _count = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        }
    }

    private DrugType _type = null!;

    public DrugType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
        }
    }

    private byte[] _photo = null!;

    public byte[] Photo
    {
        get { return _photo; }
        set
        {
            _photo = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Photo)));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
