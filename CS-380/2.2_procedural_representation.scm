#lang eopl

(define empty-env
  (lambda ()
    (lambda (search-var)
      (report-no-binding-found search-var))))

(define report-no-binding-found
  (lambda (search-var)
    (eopl:error 'apple-env "No binding for ~s" search-var)))

(define extend-env
  (lambda (saved-var saved-val saved-env)
    (lambda (search-var)
      (if (eqv? search-var saved-var)
          saved-val
          (apply-env saved-env search-var)))))

(define apply-env
  (lambda (env search-var)
    (env search-var)))

(define e
  (extend-env 'd 6
              (extend-env 'y 7
                          (empty-env))))
              