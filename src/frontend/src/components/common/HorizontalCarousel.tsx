import React, { useRef, useState, useEffect, useCallback } from "react";
import {
  Box,
  IconButton,
  useTheme,
  useMediaQuery,
  Paper,
  Typography,
} from "@mui/material";
import {
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
} from "@mui/icons-material";
import { PendencyIndicator } from "../profile/PendencyIndicator";

interface CarouselItem {
  id: string;
  label: string;
  description: string;
  icon: React.ReactNode;
  pendencyCount?: number;
}

interface HorizontalCarouselProps {
  items: CarouselItem[];
  activeItem: string;
  onItemChange: (itemId: string) => void;
  itemWidth?: number;
  showArrows?: boolean;
  showDots?: boolean;
  onPendencyClick?: (itemId: string) => void; // Added pendency click handler
}

/**
 * Componente de carrossel horizontal para navegação entre seções
 * Otimizado para mobile com touch gestures e navegação por setas
 *
 * @param items - Array de itens do carrossel
 * @param activeItem - ID do item atualmente ativo
 * @param onItemChange - Callback quando o item muda
 * @param itemWidth - Largura de cada item (padrão: 280px)
 * @param showArrows - Se deve mostrar setas de navegação
 * @param showDots - Se deve mostrar indicadores de posição
 */
export const HorizontalCarousel: React.FC<HorizontalCarouselProps> = ({
  items,
  activeItem,
  onItemChange,
  itemWidth = 280,
  showArrows = true,
  showDots = true,
  onPendencyClick,
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isXs = useMediaQuery(theme.breakpoints.down("xs"));

  const scrollContainerRef = useRef<HTMLDivElement>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [canScrollLeft, setCanScrollLeft] = useState(false);
  const [canScrollRight, setCanScrollRight] = useState(true);
  const [isDragging, setIsDragging] = useState(false);
  const [startX, setStartX] = useState(0);
  const [scrollLeft, setScrollLeft] = useState(0);
  const [isScrolling, setIsScrolling] = useState(false);

  // Ajustar largura do item baseado no breakpoint
  const getItemWidth = () => {
    if (isXs) return 260;
    if (isMobile) return 280;
    return itemWidth;
  };

  const actualItemWidth = getItemWidth();

  // Verificar se pode rolar para os lados (memoizada e otimizada)
  const checkScrollButtons = useCallback(() => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const scrollLeft = container.scrollLeft;
      const scrollWidth = container.scrollWidth;
      const clientWidth = container.clientWidth;
      
      // Usar requestAnimationFrame para evitar reflow
      requestAnimationFrame(() => {
        setCanScrollLeft(scrollLeft > 0);
        setCanScrollRight(scrollLeft < scrollWidth - clientWidth - 1);
      });
    }
  }, []);

  // Centralizar o item ativo na tela
  const centerActiveItem = () => {
    const activeIndex = items.findIndex((item) => item.id === activeItem);
    if (activeIndex !== -1 && scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const containerWidth = container.clientWidth;
      const itemWidth = actualItemWidth;
      const gap = 16; // gap entre itens

      // Calcular posição para centralizar o item
      const itemPosition = activeIndex * (itemWidth + gap);
      const scrollPosition = itemPosition - containerWidth / 2 + itemWidth / 2;

      // Limitar a posição de scroll
      const maxScroll = container.scrollWidth - containerWidth;
      const finalScrollPosition = Math.max(
        0,
        Math.min(scrollPosition, maxScroll)
      );

      setIsScrolling(true);
      container.scrollTo({
        left: finalScrollPosition,
        behavior: "smooth",
      });

      setCurrentIndex(activeIndex);

      // Resetar flag de scrolling após animação
      setTimeout(() => {
        setIsScrolling(false);
      }, 300);
    }
  };

  // Rolar para o item ativo (versão simples)
  const scrollToActiveItem = () => {
    centerActiveItem();
  };

  // Rolar para a esquerda (otimizada)
  const scrollToLeft = useCallback(() => {
    if (scrollContainerRef.current && currentIndex > 0) {
      const container = scrollContainerRef.current;
      const containerWidth = container.clientWidth;
      const itemWidth = actualItemWidth;
      const gap = 16;

      // Usar currentIndex diretamente para calcular próximo item
      const targetIndex = currentIndex - 1;

      const itemPosition = targetIndex * (itemWidth + gap);
      const scrollPosition = itemPosition - containerWidth / 2 + itemWidth / 2;
      const maxScroll = container.scrollWidth - containerWidth;
      const finalScrollPosition = Math.max(
        0,
        Math.min(scrollPosition, maxScroll)
      );

      setIsScrolling(true);
      
      // Usar requestAnimationFrame para evitar violations
      requestAnimationFrame(() => {
        container.scrollTo({
          left: finalScrollPosition,
          behavior: "smooth",
        });

        setCurrentIndex(targetIndex);

        // Atualizar item ativo
        const targetItem = items[targetIndex];
        if (targetItem) {
          onItemChange(targetItem.id);
        }

        // Usar requestAnimationFrame em vez de setTimeout
        requestAnimationFrame(() => {
          setIsScrolling(false);
        });
      });
    }
  }, [currentIndex, actualItemWidth, items, onItemChange]);

  // Rolar para a direita (otimizada)
  const scrollToRight = useCallback(() => {
    if (scrollContainerRef.current && currentIndex < items.length - 1) {
      const container = scrollContainerRef.current;
      const containerWidth = container.clientWidth;
      const itemWidth = actualItemWidth;
      const gap = 16;

      // Usar currentIndex diretamente para calcular próximo item
      const targetIndex = currentIndex + 1;

      const itemPosition = targetIndex * (itemWidth + gap);
      const scrollPosition = itemPosition - containerWidth / 2 + itemWidth / 2;
      const maxScroll = container.scrollWidth - containerWidth;
      const finalScrollPosition = Math.max(
        0,
        Math.min(scrollPosition, maxScroll)
      );

      setIsScrolling(true);
      
      // Usar requestAnimationFrame para evitar violations
      requestAnimationFrame(() => {
        container.scrollTo({
          left: finalScrollPosition,
          behavior: "smooth",
        });

        setCurrentIndex(targetIndex);

        // Atualizar item ativo
        const targetItem = items[targetIndex];
        if (targetItem) {
          onItemChange(targetItem.id);
        }

        // Usar requestAnimationFrame em vez de setTimeout
        requestAnimationFrame(() => {
          setIsScrolling(false);
        });
      });
    }
  }, [currentIndex, actualItemWidth, items, onItemChange]);


  // Função para suavizar a detecção de centro (memoizada e otimizada)
  const smoothDetectCenter = useCallback(() => {
    if (!scrollContainerRef.current || isScrolling) return;

    const container = scrollContainerRef.current;
    
    // Cache das propriedades para evitar reflow
    const containerWidth = container.clientWidth;
    const scrollLeft = container.scrollLeft;
    const itemWidth = actualItemWidth;
    const gap = 16;

    // Encontrar o item mais próximo do centro com threshold
    let closestIndex = 0;
    let minDistance = Infinity;
    const threshold = itemWidth * 0.3; // 30% da largura do item
    const containerCenter = scrollLeft + containerWidth / 2;

    // Usar for loop em vez de forEach para melhor performance
    for (let index = 0; index < items.length; index++) {
      const itemPosition = index * (itemWidth + gap);
      const itemCenter = itemPosition + itemWidth / 2;
      const distance = Math.abs(itemCenter - containerCenter);

      if (distance < minDistance) {
        minDistance = distance;
        closestIndex = index;
      }
    }

    // Só atualizar se estiver suficientemente próximo do centro
    if (minDistance < threshold && closestIndex !== currentIndex) {
      // Atualizar estado diretamente sem requestAnimationFrame para evitar delays
      setCurrentIndex(closestIndex);
      const centerItem = items[closestIndex];
      if (centerItem && centerItem.id !== activeItem) {
        onItemChange(centerItem.id);
      }
    }
  }, [
    isScrolling,
    actualItemWidth,
    items,
    currentIndex,
    activeItem,
    onItemChange,
  ]);


  // Rolar para um item específico (otimizada)
  const scrollToItem = useCallback((index: number) => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const containerWidth = container.clientWidth;
      const itemWidth = actualItemWidth;
      const gap = 16;

      // Centralizar o item
      const scrollPosition =
        index * (itemWidth + gap) - containerWidth / 2 + itemWidth / 2;

      setIsScrolling(true);
      
      // Usar requestAnimationFrame para evitar violations
      requestAnimationFrame(() => {
        container.scrollTo({
          left: Math.max(0, scrollPosition),
          behavior: "smooth",
        });

        setCurrentIndex(index);

        // Não chamar onItemChange aqui para evitar scroll automático

        // Usar requestAnimationFrame em vez de setTimeout
        requestAnimationFrame(() => {
          setIsScrolling(false);
        });
      });
    }
  }, [actualItemWidth]);

  // Mouse handlers para drag
  const handleMouseDown = (e: React.MouseEvent) => {
    setIsDragging(true);
    setStartX(e.pageX);
    if (scrollContainerRef.current) {
      setScrollLeft(scrollContainerRef.current.scrollLeft);
    }
  };

  const handleMouseMove = (e: React.MouseEvent) => {
    if (!isDragging || !scrollContainerRef.current) return;

    e.preventDefault();
    const x = e.pageX;
    const walk = (startX - x) * 2;
    scrollContainerRef.current.scrollLeft = scrollLeft + walk;
  };

  const handleMouseUp = () => {
    setIsDragging(false);
    checkScrollButtons();
    // Detecção suave após o drag
    setTimeout(() => {
      smoothDetectCenter();
    }, 150);
  };

  const handleMouseLeave = () => {
    setIsDragging(false);
    checkScrollButtons();
  };

  // Efeitos
  useEffect(() => {
    scrollToActiveItem();
  }, [activeItem]);

  useEffect(() => {
    checkScrollButtons();
  }, [items]);

  // Sincronizar currentIndex com activeItem
  useEffect(() => {
    const activeIndex = items.findIndex((item) => item.id === activeItem);
    if (activeIndex !== -1 && activeIndex !== currentIndex) {
      setCurrentIndex(activeIndex);
    }
  }, [activeItem, items, currentIndex]);

  // Listener de scroll com debounce para melhor performance
  useEffect(() => {
    const container = scrollContainerRef.current;
    if (!container) return;

    let scrollTimeout: number;

    const handleScroll = () => {
      // Usar apenas requestAnimationFrame para eliminar setTimeout
      if (scrollTimeout) {
        cancelAnimationFrame(scrollTimeout);
      }
      
      scrollTimeout = requestAnimationFrame(() => {
        if (!isScrolling) {
          smoothDetectCenter();
        }
        checkScrollButtons();
      });
    };

    container.addEventListener("scroll", handleScroll, { passive: true });

    return () => {
      if (scrollTimeout) {
        cancelAnimationFrame(scrollTimeout);
      }
      container.removeEventListener("scroll", handleScroll);
    };
  }, [isScrolling, currentIndex, activeItem]);

  // Event listeners nativos para touch com passive: false
  useEffect(() => {
    const container = scrollContainerRef.current;
    if (!container) return;

    const handleTouchStartNative = (e: TouchEvent) => {
      // Só processar se for um toque único
      if (e.touches.length === 1) {
        setIsDragging(true);
        setStartX(e.touches[0].pageX);
        if (scrollContainerRef.current) {
          setScrollLeft(scrollContainerRef.current.scrollLeft);
        }
      }
    };

    const handleTouchMoveNative = (e: TouchEvent) => {
      if (!isDragging || !scrollContainerRef.current) return;

      // Prevenir scroll padrão do navegador
      e.preventDefault();
      e.stopPropagation();

      const x = e.touches[0].pageX;
      const walk = (startX - x) * 2;
      scrollContainerRef.current.scrollLeft = scrollLeft + walk;
    };

    const handleTouchEndNative = () => {
      setIsDragging(false);
      checkScrollButtons();
      setTimeout(() => {
        smoothDetectCenter();
      }, 100);
    };

    // Adicionar listeners com passive: false para permitir preventDefault
    container.addEventListener("touchstart", handleTouchStartNative, {
      passive: false,
    });
    container.addEventListener("touchmove", handleTouchMoveNative, {
      passive: false,
    });
    container.addEventListener("touchend", handleTouchEndNative, {
      passive: false,
    });

    return () => {
      container.removeEventListener("touchstart", handleTouchStartNative);
      container.removeEventListener("touchmove", handleTouchMoveNative);
      container.removeEventListener("touchend", handleTouchEndNative);
    };
  }, [isDragging, startX, scrollLeft, checkScrollButtons, smoothDetectCenter]);

  return (
    <Box sx={{ position: "relative", width: "100%" }}>
      {/* Setas de navegação */}
      {showArrows && (
        <>
          <IconButton
            onClick={scrollToLeft}
            disabled={!canScrollLeft}
            sx={{
              position: "absolute",
              left: -20,
              top: "50%",
              transform: "translateY(-50%)",
              zIndex: 2,
              bgcolor: "background.paper",
              boxShadow: 2,
              "&:hover": {
                bgcolor: "background.paper",
              },
              "&.Mui-disabled": {
                opacity: 0.3,
              },
            }}
          >
            <ChevronLeftIcon />
          </IconButton>

          <IconButton
            onClick={scrollToRight}
            disabled={!canScrollRight}
            sx={{
              position: "absolute",
              right: -20,
              top: "50%",
              transform: "translateY(-50%)",
              zIndex: 2,
              bgcolor: "background.paper",
              boxShadow: 2,
              "&:hover": {
                bgcolor: "background.paper",
              },
              "&.Mui-disabled": {
                opacity: 0.3,
              },
            }}
          >
            <ChevronRightIcon />
          </IconButton>
        </>
      )}

      {/* Container do carrossel */}
      <Box
        ref={scrollContainerRef}
        onMouseDown={handleMouseDown}
        onMouseMove={handleMouseMove}
        onMouseUp={handleMouseUp}
        onMouseLeave={handleMouseLeave}
        sx={{
          display: "flex",
          overflowX: "auto",
          scrollBehavior: "smooth",
          scrollbarWidth: "none", // Firefox
          "&::-webkit-scrollbar": {
            display: "none", // Chrome, Safari
          },
          gap: 2,
          px: 1,
          py: 1,
          cursor: isDragging ? "grabbing" : "grab",
          "&:active": {
            cursor: "grabbing",
          },
          // Melhorar suavidade das transições
          scrollSnapType: "x mandatory",
          scrollPadding: "0 16px",
        }}
      >
        {items.map((item) => {
          const isActive = item.id === activeItem;
          return (
            <Paper
              key={item.id}
              onClick={() => onItemChange(item.id)}
              sx={{
                minWidth: actualItemWidth,
                maxWidth: actualItemWidth,
                p: 2,
                cursor: "pointer",
                transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                border: isActive
                  ? `2px solid ${theme.palette.primary.main}`
                  : "2px solid transparent",
                boxShadow: isActive ? `0 4px 12px rgba(13, 71, 161, 0.15)` : 1,
                backgroundColor: isActive
                  ? theme.palette.primary.light + "10"
                  : "background.paper",
                // Scroll snap para suavidade
                scrollSnapAlign: "center",
                scrollSnapStop: "always",
                "&:hover": {
                  boxShadow: 2,
                  transform: "translateY(-2px)",
                },
                "&:active": {
                  transform: "translateY(0px)",
                },
              }}
            >
              <Box sx={{ textAlign: "center" }}>
                {/* Ícone */}
                <Box
                  sx={{
                    color: isActive
                      ? theme.palette.primary.main
                      : "text.secondary",
                    display: "flex",
                    justifyContent: "center",
                    mb: 1,
                    fontSize: "1.5rem",
                  }}
                >
                  {item.icon}
                </Box>

                {/* Título */}
                <Typography
                  variant="subtitle2"
                  sx={{
                    fontWeight: "bold",
                    color: isActive
                      ? theme.palette.primary.main
                      : "text.primary",
                    mb: 0.5,
                    fontSize: isXs ? "0.8rem" : "0.875rem",
                  }}
                >
                  {item.label}
                </Typography>

                {/* Descrição */}
                <Typography
                  variant="caption"
                  color={isActive ? "primary" : "text.secondary"}
                  sx={{
                    display: "block",
                    fontSize: isXs ? "0.7rem" : "0.75rem",
                    lineHeight: 1.2,
                    mb: 1,
                  }}
                >
                  {item.description}
                </Typography>

                {/* Indicador de pendência ou status */}
                <PendencyIndicator
                  count={item.pendencyCount || 0}
                  size="small"
                  showSuccess={true}
                  clickable={!!onPendencyClick && (item.pendencyCount || 0) > 0}
                  onClick={
                    onPendencyClick ? () => onPendencyClick(item.id) : undefined
                  }
                />
              </Box>
            </Paper>
          );
        })}
      </Box>

      {/* Indicadores de posição (dots) */}
      {showDots && items.length > 1 && (
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            gap: 1,
            mt: 2,
          }}
        >
          {items.map((_, index) => (
            <Box
              key={index}
              onClick={() => {
                // Usar requestAnimationFrame para evitar violations
                requestAnimationFrame(() => {
                  scrollToItem(index);
                });
              }}
              sx={{
                width: 8,
                height: 8,
                borderRadius: "50%",
                backgroundColor:
                  index === currentIndex
                    ? theme.palette.primary.main
                    : theme.palette.grey[400],
                cursor: "pointer",
                transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
                transform: index === currentIndex ? "scale(1.2)" : "scale(1)",
                "&:hover": {
                  backgroundColor:
                    index === currentIndex
                      ? theme.palette.primary.dark
                      : theme.palette.grey[600],
                  transform: "scale(1.1)",
                },
              }}
            />
          ))}
        </Box>
      )}
    </Box>
  );
};

export default HorizontalCarousel;
