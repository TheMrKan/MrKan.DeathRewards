﻿using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MrKan.DeathRewards
{
    public class Config : IRocketPluginConfiguration
    {
        public int Cooldown { get; set; }
        public List<Group> Groups { get; set; } = new();
        public void LoadDefaults()
        {
            Cooldown = 60;
            Groups = new()
            {
                new Group()
                {
                    PermissionGroups = new()
                    {
                        "default",
                        "vip"
                    },
                    Items = new()
                    {
                        new Item() { Id = 15, Count = 2 }
                    }
                }
            };
        }
    }

    public class Group
    {
        [XmlArrayItem(ElementName = "Group")]
        public List<string> PermissionGroups { get; set; } = new();

        [XmlArrayItem(ElementName = "Item")]
        public List<Item> Items { get; set; } = new();
    }

    public class Item
    {
        [XmlAttribute]
        public ushort Id { get; set; }

        [XmlAttribute]
        public byte Count { get; set; }
    }
}
