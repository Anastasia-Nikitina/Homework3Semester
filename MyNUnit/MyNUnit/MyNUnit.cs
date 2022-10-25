namespace MyNUnit;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Attributes;
using System;

/// <summary>
/// Class for running tests
/// </summary>
public class MyNUnit
{
    /// <summary>
    /// Runs tests and outputs information
    /// </summary>
    public List<InformationAboutTest> Start(string path)
    {
        var result = new ConcurrentBag<InformationAboutTest>();
        var dllFiles = Directory.GetFiles(path, "*.dll");
        Parallel.ForEach(dllFiles, file =>
        {
            var types = Assembly.LoadFrom(file).GetTypes();
            Parallel.ForEach(types, type =>
            {
                var listOfMethods = SplitMethodsIntoAttributes(type);
                try
                {
                    RunAnyMethods(listOfMethods.BeforeClass, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"BeforeClass method returned exception ({e.Message}. Tests in class won't be run.");
                    return;
                }

                Parallel.ForEach (listOfMethods.Test, test =>
                {
                    var instance = Activator.CreateInstance(type);
                    try
                    {
                        RunAnyMethods(listOfMethods.Before, instance);
                    }
                    catch (Exception e)
                    {
                        result.Add(new InformationAboutTest(test.Name, "Errored: Before method returned exception.", 0, e.Message));
                        return;
                    }
                    var currentResult = RunTestAndOutInfo(test, instance);
                    try
                    {
                        RunAnyMethods(listOfMethods.After, instance);
                    }
                    catch (Exception e)
                    {
                        result.Add(new InformationAboutTest(test.Name, "Errored: After method returned exception", 0, e.Message));
                        return;
                    }
                    
                    result.Add(currentResult);
                });
                try
                {
                    RunAnyMethods(listOfMethods.AfterClass, null);
                }
                catch (Exception e)
                {
                    foreach (var res in result)
                    {
                        res.Result = "Errored: AfterClass method returned exception";
                    } ;
                }
            });
        });
        return result.ToList();
    }

    /// <summary>
    /// Prints information about test
    /// </summary>
    public void RunAndPrintInfo(string path)
    {
        var result = Start(path);
        result.ForEach(inf => Console.WriteLine($"Test '{inf.Name}' {inf.Result}. Time: {inf.Time}. {inf.ReasonOfIgnore} "));
    }

    private static void RunAnyMethods(List<MethodInfo> methods, object? instance)
    {
        foreach (var method in methods)
        {
            method.Invoke(instance, null);
        }
    }
    
    private static Exception? RunTestMethod(MethodBase method, object? classInstance)
    {
        try
        {
            method.Invoke(classInstance, null);
        }
        catch (Exception e)
        {
            return e;
        }
        return null;
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
            if (result == null)
            {
                return new InformationAboutTest(testMethod.Name, "Failed: expected exception, but test was passed", stopWatch.ElapsedMilliseconds, "");
            }
            if (result.InnerException?.GetType() != argumentOfAttribute.Expected)
            { 
                return new InformationAboutTest(testMethod.Name, $"Failed: expected exception {argumentOfAttribute.Expected}, but occured {result.InnerException?.GetType()}", stopWatch.ElapsedMilliseconds, "");
            }
            return new InformationAboutTest(testMethod.Name, "Passed", stopWatch.ElapsedMilliseconds, "");
        }

        if (result != null)
        {
            return new InformationAboutTest(testMethod.Name, $"Failed: occured exception: {result.InnerException?.GetType()}", stopWatch.ElapsedMilliseconds, ""); 
        }
        return new InformationAboutTest(testMethod.Name, "Passed", stopWatch.ElapsedMilliseconds, "");
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
                throw new IncorrectMethodException("Test, Before and After methods must be non-static and mustn't return or take values.");
            }
        }
        else if (typeOfAttribute == typeof(BeforeClass) || typeOfAttribute == typeof(AfterClass))
        {
            if (!method.IsStatic)
            {
                throw new IncorrectMethodException("BeforeClass and AfterClass methods must be static");
            }
        }
    }
}
 