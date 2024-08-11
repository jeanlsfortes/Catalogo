namespace MyAPI.Logging
{
    public class CustomerLogger : ILogger
    {
        private readonly string loggerName;
        private readonly CustomLoggerProviderConfiguration loggerConfig;
        private readonly string logFilePath = @"d:\dados\log\Macoratti_Log.txt";

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable? BeginScope<TState>(TState state) => null;

        IDisposable? ILogger.BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string mensagem = $"{DateTime.Now} [{logLevel}] {eventId.Id} - {formatter(state, exception)}";

            try
            {
                EscreverTextoNoArquivo(mensagem);
            }
            catch (Exception ex)
            {
                // Log de falha pode ser tratado aqui ou em um sistema de monitoramento
                Console.Error.WriteLine($"Falha ao escrever no arquivo de log: {ex.Message}");
            }
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, 
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string mensagem = $"{DateTime.Now} [{logLevel}] {eventId.Id} - {formatter(state, exception)}";

            try
            {
                EscreverTextoNoArquivo(mensagem);
            }
            catch (Exception ex)
            {
                // Log de falha pode ser tratado aqui ou em um sistema de monitoramento
                Console.Error.WriteLine($"Falha ao escrever no arquivo de log: {ex.Message}");
            }
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
                {
                    streamWriter.WriteLine(mensagem);
                }
            }
            catch (IOException ioEx)
            {
                // Tratamento específico para erros de IO
                Console.Error.WriteLine($"Erro de IO ao tentar escrever no arquivo: {ioEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Outros tipos de exceções
                Console.Error.WriteLine($"Erro ao tentar escrever no arquivo: {ex.Message}");
                throw;
            }
        }
    }
}
