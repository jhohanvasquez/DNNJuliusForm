using AutoMapper;
using System.Collections.Generic;

namespace DNNJuliusForm.Common.Utility
{
    public class AutoMapp<T, T2>
    {
        public static T2 Convert(T obj)
        {
            Mapper.CreateMap<T, T2>();
            return Mapper.Map<T, T2>(obj);
        }

        public static List<T2> ConvertList(List<T> obj)
        {
            Mapper.CreateMap<List<T>, List<T2>>();
            return Mapper.Map<List<T>, List<T2>>(obj);
        }
    }
}
