# Заданиякмодулю Expressions and IQueryable

# ExpressionTree

# Задание 1.

Создайте класс-трансформатор на основе ExpressionVisitor, выполняющий следующие 2 вида преобразований дерева выражений:

- Замену выражений вида &lt;переменная&gt; + 1 / &lt;переменная&gt; - 1 на операции инкремента и декремента
- Замену параметров, входящих в lambda-выражение, на константы (в качестве параметров такого преобразования передавать:
  - Исходное выражение
  - Список пар &lt;имя параметра: значение для замены&gt;

Для контроля полученное дерево выводить в консоль или смотреть результат под отладчиком, использую ExpressionTree Visualizer, а также компилировать его и вызывать полученный метод.

## Задание 2.

Используя возможность конструировать ExpressionTree и выполнять его код, создайте собственный механизм маппинга (копирующего поля (свойства) одного класса в другой).

Приблизительный интерфейс и пример использования приведен ниже (MapperGenerator – фабрика мапперов, Mapper – класс маппинга). Обратите внимание, что в данном примере создается только новый экземпляр конечного класса, но сами данные не копируются.

public class Mapper&lt;TSource, TDestination&gt;
{
    Func&lt;TSource, TDestination&gt; mapFunction;
    internal Mapper(Func&lt;TSource, TDestination&gt; func)
    {
        mapFunction = func;
    }
    public TDestination Map(TSource source)
    {
        return mapFunction(source);
    }
}
public class MappingGenerator
{
    public Mapper&lt;TSource, TDestination&gt; Generate&lt;TSource, TDestination&gt;()
    {
        var sourceParam = Expression.Parameter(typeof(TSource));
        var mapFunction =
            Expression.Lambda&lt;Func&lt;TSource, TDestination&gt;&gt;(
            Expression.New(typeof(TDestination)),
            sourceParam
            );

        return new Mapper&lt;TSource, TDestination&gt;(mapFunction.Compile());
    }
}
public class Foo { }
public class Bar { }

[TestMethod]
public void TestMethod3()
{
    var mapGenerator = new MappingGenerator();
    var mapper = mapGenerator.Generate&lt;Foo, Bar&gt;();

    var res = mapper.Map(new Foo());
}

# IQueryable

## Задание 1.

Доработайте приведенный на лекции LINQ провайдер.

В частности, требуется добавить следующее:

- Снять текущее ограничение на порядок операндов выражения. Должны быть допустимы:
  - &lt;имя фильтруемого поля&gt; == &lt;константа&gt; (сейчас доступен только этот)
  - &lt;константа&gt; == &lt;имяфильтруемого поля&gt;
- Добавить поддержку операций включения (т.е. не точное совпадение со строкой, а частичное). При этом в LINQ-нотации они должны выглядеть как обращение к методам класса string: StartsWith, EndsWith, Contains, а точнее

| Выражение | Транслируется в запрос |
| --- | --- |
| Where(e =&gt; e.workstation.StartsWith(&quot;EPRUIZHW006&quot;)) | workstation:(EPRUIZHW006\*) |
| Where(e =&gt; e.workstation.EndsWith(&quot;IZHW0060&quot;)) | workstation:(\*IZHW0060) |
| Where(e =&gt; e.workstation.Contains(&quot;IZHW006&quot;)) | workstation:(\*IZHW006\*) |

- Добавить поддержку оператора AND (потребует доработки также самого E3SQueryClient). Организацию оператора AND в запросе к E3S смотрите [на странице документации](https://kb.epam.com/display/E3S/E3S+public+REST+for+data) (раздел FTS Request Syntax)