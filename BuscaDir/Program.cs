
        string? nome = Console.ReadLine();

        Console.WriteLine("Digite o caminho base onde deseja procurar:");
        string? caminhoBase = Console.ReadLine();

        Console.WriteLine("Digite a profundidade máxima (ex: 2):");
        int profundidadeMaxima = int.Parse(Console.ReadLine());

        bool encontrado = Buscar(caminhoBase, nome, 0, profundidadeMaxima);

        if (!encontrado)
        {
            Console.WriteLine("false - Não encontrado");
        }
    

    static bool Buscar(string caminho, string nome, int nivelAtual, int profundidadeMaxima)
    {
        if (nivelAtual > profundidadeMaxima) return false;

        try
        {
            // Verifica arquivos
            foreach (string arquivo in Directory.GetFiles(caminho))
            {
                string nomeArquivo = Path.GetFileName(arquivo);
                if (nomeArquivo.Equals(nome, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"true - Arquivo encontrado: {arquivo}");
                    return true;
                }
                else
                {
                    int distancia = DistanciaLevenshtein(nome.ToLower(), nomeArquivo.ToLower());
                    if (distancia <= 2)
                    {
                        Console.WriteLine($"Parecido com '{nome}': {arquivo}");
                        return true;
                    }
                }
            }

            // Verifica pastas
            foreach (string pasta in Directory.GetDirectories(caminho))
            {
                string nomePasta = Path.GetFileName(pasta);
                if (nomePasta.Equals(nome, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"true - Pasta encontrada: {pasta}");
                    return true;
                }
                else
                {
                    int distancia = DistanciaLevenshtein(nome.ToLower(), nomePasta.ToLower());
                    if (distancia <= 2)
                    {
                        Console.WriteLine($"Parecido com '{nome}': {pasta}");
                        return true;
                    }
                }

                // Busca recursiva em subpastas
                if (Buscar(pasta, nome, nivelAtual + 1, profundidadeMaxima))
                    return true;
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Ignora pastas sem permissão
        }

        return false;
    }

    // Função de distância de Levenshtein
    static int DistanciaLevenshtein(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int custo = (a[i - 1] == b[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + custo
                );
            }
        }

        return dp[a.Length, b.Length];
    }

