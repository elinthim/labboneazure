using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
 

namespace CombinedExample

{

    class Program

    {

        static void Main(string[] args)

        {

            // Question Answering Setup

            Uri qaEndpoint = new Uri("https://languageservicewesteuropet.cognitiveservices.azure.com/");
    
            AzureKeyCredential qaCredential = new AzureKeyCredential("6a9fb555102a4d39a3560e848f190836");                                                                    

        
            
            string projectName = "LearnFAQ";

            string deploymentName = "production";

            QuestionAnsweringClient qaClient = new QuestionAnsweringClient(qaEndpoint, qaCredential);

            QuestionAnsweringProject qaProject = new QuestionAnsweringProject(projectName, deploymentName);

 

            // Sentiment Analysis Setup this is NLP

            string languageKey = "f1b154e183af4c118f7a2f28f85f769b";

            string languageEndpoint = "https://cogservicenortheurope.cognitiveservices.azure.com/";

            AzureKeyCredential sentimentCredentials = new AzureKeyCredential(languageKey);

            Uri sentimentEndpoint = new Uri(languageEndpoint);

            TextAnalyticsClient sentimentClient = new TextAnalyticsClient(sentimentEndpoint, sentimentCredentials);

 

            var question = "";

            while (question.ToLower() != "x")

            {

                Console.WriteLine("Hello my little friend what do you want help with today? Or write x to exit");

                question = Console.ReadLine();

 

                // Question Answering

                Response<AnswersResult> qaResponse = qaClient.GetAnswers(question, qaProject);

                foreach (KnowledgeBaseAnswer answer in qaResponse.Value.Answers)

                {

                    Console.WriteLine($"Q&A: {answer.Answer}");

                }
 

                // Sentiment Analysis

                var documents = new List<string> { question };

                AnalyzeSentimentResultCollection reviews = sentimentClient.AnalyzeSentimentBatch(documents, options: new AnalyzeSentimentOptions()

                {

                    IncludeOpinionMining = true

                });
 

                foreach (AnalyzeSentimentResult review in reviews)

                {

                    var ds = review.DocumentSentiment;

                    Console.WriteLine($"Sentiment: {ds.Sentiment}");

                    Console.WriteLine($"Positive score: {ds.ConfidenceScores.Positive:0.00}");

                    Console.WriteLine($"Negative score: {ds.ConfidenceScores.Negative:0.00}");

                    Console.WriteLine($"Neutral score: {ds.ConfidenceScores.Neutral:0.00}");

                    Console.WriteLine();

                }

            }

        }

    }

}
