using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace magartubi.Classes.UtiliMagazzino
{
    public class Missione
    {
        public List<TargetMissione> targets { get; set; }
        //public List<string> ordini_presenti { get; private set; }

        public Missione(List<TargetMissione> _targets)
        {
            targets = _targets;
            //ordini_presenti = new List<string>();
            //controllo quanti ordini sono presenti nella lista
            //foreach(TargetMissione t in _targets)
            //{
            //    string s = t.GetStringaOrdine();
            //    if (!ordini_presenti.Contains(s))
            //        ordini_presenti.Add(s);
            //}
        }
        //costruttore di copia
        public Missione(Missione m)
        {
            targets = new List<TargetMissione>(m.targets);
            //ordini_presenti = new List<string>();
            //ordini_presenti = m.ordini_presenti;
        }
        /// <summary>
        /// ritorna una copia della lista targets della missione
        /// </summary>
        /// <returns></returns>
        public List<TargetMissione> GetTargets()
        {
            return new List<TargetMissione>(targets);
        }
        public void SetPosPrelievoTarget(string _cod_art,int _pos_prelievo)
        {
            bool articoli_prelevati = false;
            bool articoli_gia_prelevati = false;
            Console.WriteLine("codice articolo "+_cod_art);
            Console.WriteLine("posizione di prelievo "+_pos_prelievo);
            for (int i = 0; i < targets.Count && !articoli_gia_prelevati; i++)
            {
                if (targets[i].cod_art == _cod_art && targets[i].pos_prelievo != -1)
                    articoli_gia_prelevati = true;

                if (targets[i].cod_art == _cod_art && targets[i].pos_prelievo == -1)
                {
                    targets[i].pos_prelievo = _pos_prelievo;
                    articoli_prelevati = true;
                }
            }
            if (articoli_gia_prelevati)
                throw new EMissionePrelievoFallito("Gli articoli sono gia' stati prelevati");
            if (!articoli_prelevati)
                throw new EMissionePrelievoFallito("Gli articoli da prelevare non fanno parte della missione");
        }
        //una missione vuota viene considerata come completata
        public bool MissioneIsCompletata()
        {
            bool sent = false;
            for (int i = 0; i < targets.Count && !sent; i++)
            {
                if (targets[i].pos_prelievo == -1)
                    sent = true;
            }
            if (sent)
                return false;
            return true;
        }
        /// <summary>
        /// imposta le posizioni di prelievo di tutti i targets missione a -1
        /// </summary>
        public void Ripristina()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].pos_prelievo = -1;
            }
        }

    }

}
