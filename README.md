# WatchableData
[![GitHub license](https://img.shields.io/github/license/incognito201/WatchableData)](https://github.com/incognito201/WatchableData/blob/master/LICENSE)
[![GitHub stars](https://img.shields.io/github/stars/incognito201/WatchableData)](https://github.com/incognito201/WatchableData/stargazers)
[![NuGet](https://img.shields.io/nuget/v/WatchableData)](https://www.nuget.org/packages/WatchableData)

Constrói observables a partir de objetos que implementam INotifyPropertyChanged, INotifyCollectionChanged ou INotifyDataErrorInfo.

## Get Started
### 1. Instalação
Use o NuGet Package Manager para instalar o pacote ou use o comando a seguir no NuGet Package Manager Console.
```	
PM> Install-Package WatchableData
```

## Um Exemplo
- Monitorando múltiplas propriedades de um item
```cs
public decimal Total
{
    get => OrderItems.Sum(i => i.Quantity * i.Price);
}
```
```cs
OrderItems = new WatchableCollection<OrderItem>();

OrderItems.WhenAnyItemPropertyChanged(x => x.Quantity, x => x.Price)
    .Subscribe(i =>
    {
        RaisePropertyChanged(nameof(Total));
    });
```

--------------------------------
**Atenção:** Os observables precisam ser descartados após a inscrição para evitar vazamento de memória.

## Considerações
Essa biblioteca foi desenvolvida para ser utilizada em sistemas simples ou legados. Se você utiliza .NET Framework 4.6.1 ou superior considere utilizar ReactiveUI ou outra biblioteca mais robusta.
