﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using QuestomAssets.AssetsChanger;
using QuestomAssets.Utils;

namespace QuestomAssets
{
    public static class Extensions
    {

        public static T DeepClone<T>(this IObjectInfo<T> source, AssetsFile toFile = null, List<CloneExclusion> exclusions = null, List<AssetsObject> addedObjects = null) where T : AssetsObject
        {
            return Cloner.DeepClone<T>((T)source.Object, toFile, addedObjects, null, exclusions);
        }


        public static byte[] ToPngBytes(this Texture2DObject texture)
        {
            return ImageUtils.Instance.TextureToPngBytes(texture);
        }

        public static string RemoveSuffix(this string value, string suffix)
        {
            if (!value.EndsWith(suffix))
                return value;
            //don't remove it if that's all there is
            if (value == suffix)
                return value;

            return value.Substring(0, value.Length - suffix.Length);
        }

    }
}
