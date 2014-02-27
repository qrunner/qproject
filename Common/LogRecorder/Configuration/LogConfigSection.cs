using Provider.Configuration;
using System;
using System.Configuration;

namespace Provider.Logging.Configuration
{
    /// <summary>
    /// Конфигурация логирования
    /// </summary>
    public class LogConfigSection : ConfigurationSection
    {
        public const string _masterPrefix = "log";

        const string _levels = "levels";
        [ConfigurationProperty(_levels)]
        public LogLevelsConfigCollection Levels { get { return base[_levels] as LogLevelsConfigCollection; } }

        /*const string _listeners = "listeners";
        [ConfigurationProperty(_listeners)]
        public LogListenerConfigCollection Listeners { get { return ((LogListenerConfigCollection)(base[_listeners])); } }
        */

        /*const string _defaultProvider = "defaultListener";
        [ConfigurationProperty(_defaultProvider, IsKey = false, IsRequired = true)]
        public string DefaultListenerName
        {
            get { return ((string)(base[_defaultProvider])); }
            set { base[_defaultProvider] = value; }
        }*/

        const string _emergencyProvider = "emergencyProvider";
        [ConfigurationProperty(_emergencyProvider, IsKey = false, IsRequired = true)]
        public string EmergencyProviderName
        {
            get { return (string)base[_emergencyProvider]; }
            set { base[_emergencyProvider] = value; }
        }

        //public LogListenerConfig DefaultListener { get { return Listeners[DefaultListenerName]; } }
       // public LogListenerConfig EmergencyListener { get { return Listeners[EmergencyListenerName]; } }
    }
    /// <summary>
    /// Конфигурация уровней логирования
    /// </summary>
    public class LogLevelsConfig : NamedConfigurationElement
    {        
        public LogLevel Code
        {
            get { return (LogLevel)Enum.Parse(typeof(LogLevel), (string)base.Name, true); }
            set { base.Name = Enum.GetName(typeof(LogLevel), value); }
        }

        const string _enabled = "enabled";
        [ConfigurationProperty(_enabled, DefaultValue = "true", IsKey = false, IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)base[_enabled]; }
            set { base[_enabled] = value; }
        }
        const string _activeProviders = "activeProviders";
        [ConfigurationProperty(_activeProviders)]
        public LevelListenersConfigCollection ActiveProviders { get { return base[_activeProviders] as LevelListenersConfigCollection; } }
    }
    /// <summary>
    /// Коллекция конфигурация уровней логирования
    /// </summary>
    [ConfigurationCollection(typeof(LogLevelsConfig), AddItemName = "level")]
    public class LogLevelsConfigCollection : ConfigurationElementCollectionBase<LogLevelsConfig> { }
    /// <summary>
    /// Прослушиватели уровня логирования
    /// </summary>
    //public class LevelListenersConfig : NamedConfigurationElement {}  
    /// <summary>
    /// Коллекция прослушивателей уровня логирования
    /// </summary>
    [ConfigurationCollection(typeof(NamedConfigurationElement), AddItemName = "add")]
    public class LevelListenersConfigCollection : ConfigurationElementCollectionBase<NamedConfigurationElement> { }
    /*
    /// <summary>
    /// Конфигурация прослушивателя лога
    /// </summary>
    public class LogListenerConfig : NamedConfigurationElement
    {
        const string _providerName = "providerName";
        [ConfigurationProperty(_providerName, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string ProviderName
        {
            get { return (string)base[_providerName]; }
            set { base[_providerName] = value; }
        }

        const string _bufferCapacity = "bufferCapacity";
        [ConfigurationProperty(_bufferCapacity, DefaultValue = "10", IsKey = false)]
        public int BufferCapacity
        {
            get { return (int)base[_bufferCapacity]; }
            set { base[_bufferCapacity] = value; }
        }

        const string _flushInterval = "flushInterval";
        [ConfigurationProperty(_flushInterval, DefaultValue = "10000", IsKey = false)]
        public int FlushInterval
        {
            get { return (int)base[_flushInterval]; }
            set { base[_flushInterval] = value; }
        }

        const string _settings = "settings";
        [ConfigurationProperty(_settings)]
        public KeyValueConfigurationCollection Settings { get { return base[_settings] as KeyValueConfigurationCollection; } }
    }
    /// <summary>
    /// Коллекция конфигураций прослушивателей лога
    /// </summary>
    [ConfigurationCollection(typeof(LogListenerConfig), AddItemName = "listener")]
    public class LogListenerConfigCollection : ConfigurationElementCollectionBase<LogListenerConfig> { }
    */
    /*
    public class LogListenerConfigCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogListenerConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogListenerConfig)(element)).Name;
        }

        public LogListenerConfig this[int idx]
        {
            get { return (LogListenerConfig)BaseGet(idx); }
        }

        public LogListenerConfig this[string name]
        {
            get { return (LogListenerConfig)BaseGet(name); }
        }
    }
    */
    /*/// <summary>
    /// Коллекция целей для лога
    /// </summary>
    [ConfigurationCollection(typeof(LogTargetElement), AddItemName = LogConfigSection._Targets)]
    public class LogTargetsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogTargetElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogTargetElement)(element)).ConnectionStringName;
        }

        public LogTargetElement this[int idx]
        {
            get { return (LogTargetElement)BaseGet(idx); }
        }

        public LogTargetElement this[string name]
        {
            get { return (LogTargetElement)BaseGet(name); }
        }
    }
    /// <summary>
    /// Цели логирования
    /// </summary>
    public class LogTargetElement : ConfigurationElement
    {
        const string _csName = "connectionStringName";
        [ConfigurationProperty(_csName, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ConnectionStringName
        {
            get { return ((string)(base[_csName])); }
            set { base[_csName] = value; }
        }
        const string _name = "name";
        [ConfigurationProperty(_name, DefaultValue = "", IsKey = false, IsRequired = true)]
        public string TargetName
        {
            get { return ((string)(base[_name])); }
            set { base[_name] = value; }
        }
    }
    /*
    /// <summary>
    /// Уровень логирования
    /// </summary>
    public class LogLevelElement : ConfigurationElement
    {
        const string _level = "level";
        [ConfigurationProperty(_level, DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Level
        {
            get { return ((string)(base[_level])); }
            set { base[_level] = value; }
        }
        const string _enabled = "enabled";
        [ConfigurationProperty(_enabled, DefaultValue = "", IsKey = false, IsRequired = true)]
        public bool Enabled
        {
            get { return ((bool)(base[_enabled])); }
            set { base[_enabled] = value; }
        }

        const string _levelTargets = "LevelTargets";
        [ConfigurationProperty(_levelTargets)]
        public ConfigurationElementCollection LevelTargets
        {
            get { return (ConfigurationElementCollection)base[_levelTargets]; }
            set { base[_levelTargets] = value; }
        }
        /*
        public IEnumerable<LogTargetElement> Targets
        {
            get
            {
                HashSet<LogTargetElement> retval = new HashSet<LogTargetElement>();
                foreach (ConfigurationElement elem in LevelTargets)
                {
                    retval.Add((elem.CurrentConfiguration as LogConfigSection).LogTargets[elem.ElementInformation.Properties["name"].Value.ToString()]);
                }
                return retval;
            }
        }
         * */
    /*}
    */
}