using HtmlAgilityPack;
using System.Threading.Tasks;
using System;

public class Parser
{
    private readonly HtmlWeb _web;

    public Parser(HtmlWeb web)
    {
        _web = web;
    }

    public async Task<string> GetTitleAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);

        var titleNode = document.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim();

        if (titleNode == null)
        {
            throw new Exception("Title not found");
        }

        return titleNode;
    }

    public async Task<string> GetNameAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);

        var nameNode = document.DocumentNode.SelectSingleNode("//*[@id=\"container\"]/div[2]/div/div[2]/div[1]/div/div[4]/h1");
        var name = nameNode.InnerText.Trim();

        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name not found");
        }

        return name;
    }

    public async Task<string> GetDescriptionAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);

        var descriptionNode = document.DocumentNode.SelectSingleNode("//*[@id=\"container\"]/div[2]/div/div[2]/div[1]/div/div[4]/div[2]/p");
        var description = descriptionNode.InnerText.Trim();

        if (string.IsNullOrEmpty(description))
        {
            throw new Exception("Name not found");
        }

        return description;
    }

    public async Task<string> GetPriceAsync(string url)
    {
        var document = await _web.LoadFromWebAsync(url);

        var priceNode = document.DocumentNode.SelectSingleNode("//span[@class='salary']");
        var salary = priceNode.InnerText.Trim();

        if (string.IsNullOrEmpty(salary))
        {
            throw new Exception("Salry not found");
        }

        return salary;
    }
}

public class Job
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Salary { get; set; }

    public override string ToString()
    {
        return $"Title: {Title} \n\nName: {Name} \n\nDescription: {Description} \n\nSalary: {Salary}";
    }
}

public static class Program
{
    private const string JobUrl =
        "https://jobs.dou.ua/companies/softheme/vacancies/265514/?from=widget_hot";

    static async Task Main(string[] args)
    {
        var parser = new Parser(new HtmlWeb());

        var job = new Job()
        {
            Title = await parser.GetTitleAsync(JobUrl),
            Name = await parser.GetNameAsync(JobUrl),
            Description = await parser.GetDescriptionAsync(JobUrl),
            Salary = await parser.GetPriceAsync(JobUrl),
        };
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(job);
    }
}


