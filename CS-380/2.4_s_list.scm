#lang eopl

;S-list: ({S-exp}*) //({S-exp},{S-exp},{S-exp}...)
;S-exp : Symbol | S-list

(define-datatype s-list s-list?
  (empty-s-list)
  (non-empty-s-list
   (first s-exp?)
   (rest s-list?)))

(define-datatype s-exp s-exp?
  (sym-s-exp
   (sym symbol?))
   (s-list-s-exp
    (slist s-list?)))

(define found?
  (lambda (search-var slst)
    (cases s-list slst
      (empty-s-list () #t)
      (non-empty-s-list (first rest)
                        (or
                         (exp-found? search-var first)
                         (exp-found? search-var rest))))))

(define exp-found?
  (lambda (search-var s-exper)
    (cases s-exp s-exper
      (sym-s-exp (sym)
                 (eqv? sym search-var))
      (s-list-s-exp (slist)
                    (eqv? slist search-var)))))