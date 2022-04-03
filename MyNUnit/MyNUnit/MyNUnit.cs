namespace MyNUnit;

using System.Diagnostics;
using System.Reflection;
using Attributes;

public class MyNUnit
{
    public List<InformationAboutTest> Start(string path)
    {
        var result = new List<InformationAboutTest>();
        var dllFiles = Directory.GetFiles(path, "*.dll");
        Parallel.ForEach(dllFiles, file =>
        {
            var types = Assembly.LoadFrom(file).GetTypes();
            Parallel.ForEach(types, type =>
            {
                var listOfMethods = SplitMethodsIntoAttributes(type);
                var instance = Activator.CreateInstance(type);
                RunAnyMethods(listOfMethods.BeforeClass, null!);
                foreach (var test in listOfMethods.Test)
                {
                    RunAnyMethods(listOfMethods.Before, instance);
                    var currentResult = RunTestAndOutInfo(test, instance);
                    result.Add (currentResult);
                    RunAnyMethods(listOfMethods.After, instance);
                }

                RunAnyMethods(listOfMethods.AfterClass, null);
            });
        });
        return result;
    }

    public void PrintInfo(string path)
    {
        var result = Start(path);
        result.ForEach(inf => Console.WriteLine("Test '{0}' {1}. Time: {2}. {3} ", inf.Name, inf.Result, inf.Time, inf.ReasonOfIgnore));

    }

    private static void RunAnyMethods(List<MethodInfo> methods, object? instance)
    {
        foreach (var method in methods)
        {
            try
            {
                method.Invoke(instance, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
    
    private static string RunTestMethod(MethodBase method, object? classInstance)
    {
        try
        {
            method.Invoke(classInstance, null);
        }
        catch (Exception e)
        {
            return $"Failed: {e.Message}";
        }

        return "Passed";
    }

    private InformationAboutTest RunTestAndOutInfo(MethodInfo testMethod, object? instance)
    {
        var argumentOfAttribute = (Test)Attribute.GetCustomAttribute(testMethod, typeof(Test))!;
        if (argumentOfAttribute.Ignore != null)
        {
            return new InformationAboutTest(testMethod.Name, "Ignored", 0, argumentOfAttribute.Ignore);
        }

        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var result = RunTestMethod(testMethod, instance);
        stopWatch.Stop();
        
        if (argumentOfAttribute.Expected != null)
        {
            if (result != "Passed")
            {
                return new InformationAboutTest(testMethod.Name, "Passed", stopWatch.ElapsedMilliseconds, "");
            }
            return new InformationAboutTest(testMethod.Name, "Failed: expected exception", stopWatch.ElapsedMilliseconds, "");
        }
        
        return new InformationAboutTest(testMethod.Name, result, stopWatch.ElapsedMilliseconds, "");
    }

    private static TypesOfMethods SplitMethodsIntoAttributes(Type classOfTests)
    {
        var listOfMethods = new TypesOfMethods();
        var methodsInClass = classOfTests.GetMethods();
        foreach (var method in methodsInClass)
        {
            var attributesOfMethod = method.GetCustomAttributes();
            foreach (var attribute in attributesOfMethod)
            {
                CheckMethod(attribute, method);
                switch (attribute)
                {
                    case Test:
                        listOfMethods.Test.Add(method);
                        break;
                    case Before:
                        listOfMethods.Before.Add(method);
                        break;
                    case After:
                        listOfMethods.After.Add(method);
                        break;
                    case BeforeClass:
                        listOfMethods.BeforeClass.Add(method);
                        break;
                    case AfterClass:
                        listOfMethods.AfterClass.Add(method);
                        break;
                }
            }
        }
        return listOfMethods;
    }

    private static void CheckMethod(Attribute attribute, MethodInfo method)
    {
        var typeOfAttribute = attribute.GetType();
        if (typeOfAttribute == typeof(Test) || typeOfAttribute == typeof(Before) || typeOfAttribute == typeof(After) ) 
        {
            if (method.IsStatic || method.GetParameters().Length != 0 || method.ReturnType.Name != "Void")
            {
                throw new Exception("Test, Before and After methods must be non-static and mustn't return or take values.");
            }
        }
        else if (typeOfAttribute == typeof(BeforeClass) || typeOfAttribute == typeof(AfterClass))
        {
            if (!method.IsStatic)
            {
                throw new Exception("BeforeClass and AfterClass methods must be static");
            }
        }
    }
}
 