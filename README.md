# MrKan.DeathRewards

Simple RocketMod plugin for Unturned that gives a player specific items after death depending on his permission groups.

### Config
*Groups priority: the **lower** ones have more priority.*
*Parent groups are also checked.*

**Cooldown** - minimal interval between rewards in seconds. The cooldown isn't saved after server restart, but is saved after plugin reload.

**Group.PermissionGroups** - list of IDs of permission groups.

```xml
<Config xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Cooldown>60</Cooldown>
  <Groups>
    <Group>
      <PermissionGroups>
        <Group>default</Group>
        <Group>vip</Group>
      </PermissionGroups>
      <Items>
        <Item Id="15" Count="2" />
      </Items>
    </Group>
  </Groups>
</Config>	
```