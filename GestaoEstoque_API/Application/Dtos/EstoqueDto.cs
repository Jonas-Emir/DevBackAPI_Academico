﻿using GestaoEstoque_API.Application.Enums;

namespace GestaoEstoque_API.Application.Dtos
{
    public class EstoqueRequestDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public int TipoMovimentoId { get; set; } // Chave estrangeira
    }

    public class EstoqueResponseDto
    {
        public int EstoqueId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public int TipoMovimentoId { get; set; } // Chave estrangeira
        public TipoMovimento TipoMovimento { get; set; } // Propriedade de navegação
    }
}