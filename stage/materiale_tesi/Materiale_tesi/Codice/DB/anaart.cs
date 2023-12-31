//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class anaart
    {
        public string cod_art { get; set; }
        public string des_art { get; set; }
        public string tipo_art { get; set; }
        public string cod_um { get; set; }
        public string cod_iva { get; set; }
        public string tipo_gram { get; set; }
        public Nullable<double> grammatura { get; set; }
        public Nullable<int> conf_collo { get; set; }
        public Nullable<int> pezzi_conf { get; set; }
        public string tipo_conf { get; set; }
        public Nullable<double> peso_netto_collo { get; set; }
        public Nullable<double> tara_collo { get; set; }
        public Nullable<double> peso_netto_conf { get; set; }
        public Nullable<double> tara_conf { get; set; }
        public string flag_fv { get; set; }
        public Nullable<double> perc_calopeso { get; set; }
        public Nullable<double> perc_scost_bil { get; set; }
        public string ean_art { get; set; }
        public string ean_cartone { get; set; }
        public string cod_contropartita { get; set; }
        public string conto_vendite_it { get; set; }
        public string conto_vendite_es { get; set; }
        public string cod_linea_comm { get; set; }
        public string cod_sett_comm { get; set; }
        public string cod_fam_comm { get; set; }
        public string cod_sott_comm { get; set; }
        public string cod_linea_gest { get; set; }
        public string cod_sett_gest { get; set; }
        public string cod_fam_gest { get; set; }
        public string cod_sott_gest { get; set; }
        public string flag_pc { get; set; }
        public string flag_ec { get; set; }
        public string cod_stato { get; set; }
        public Nullable<System.DateTime> data_stato { get; set; }
        public string tipo_scadenza { get; set; }
        public Nullable<int> qta_tipo_scad { get; set; }
        public string tipo_utilizzo { get; set; }
        public string tipo_imballo { get; set; }
        public string tipo_vendita { get; set; }
        public Nullable<double> seq_prelievo { get; set; }
        public string ragg_prod { get; set; }
        public string cod_raf { get; set; }
        public string voce_dog { get; set; }
        public Nullable<double> cubaggio { get; set; }
        public Nullable<int> base_pallet { get; set; }
        public Nullable<int> altez_pallet { get; set; }
        public string flag_qualita { get; set; }
        public Nullable<int> volume { get; set; }
        public string flag_omaggiabile { get; set; }
        public string disegno_tec { get; set; }
        public Nullable<double> qta_lotto_produz { get; set; }
        public string cod_mag { get; set; }
        public string tipo_art_prod { get; set; }
        public Nullable<int> tempo_approv { get; set; }
        public Nullable<int> tempo_accorp { get; set; }
        public string des_agg { get; set; }
        public string tipo_um_ordcli { get; set; }
        public string flag_partite { get; set; }
        public string flag_stagionatura { get; set; }
        public Nullable<int> giorni_stag { get; set; }
        public string cod_art_stag { get; set; }
        public string contrass_bev { get; set; }
        public string flag_bloccato { get; set; }
        public string flag_da_etich { get; set; }
        public string rep_appr { get; set; }
        public Nullable<int> tempo_lav { get; set; }
        public Nullable<int> tempo_att { get; set; }
        public string tipo_lavorazione { get; set; }
        public string flag_linea_conf { get; set; }
        public string tipo_linea_conf { get; set; }
        public Nullable<int> gio_tra_sped_scad { get; set; }
        public string flag_novita { get; set; }
        public string cod_merc { get; set; }
        public string cartone_da_etich { get; set; }
        public string cod_um_prod { get; set; }
        public string flag_distributore { get; set; }
        public string flag_colli_non_div { get; set; }
        public string flag_personalizzat { get; set; }
        public string flag_norifatt { get; set; }
        public string flag_prenotazione { get; set; }
        public string flag_distr_ven_nol { get; set; }
        public string cod_linea_web { get; set; }
        public string cod_sett_web { get; set; }
        public string cod_fam_web { get; set; }
        public string cod_sott_web { get; set; }
        public Nullable<System.DateTime> data_acquisiz { get; set; }
        public string img_web { get; set; }
        public string img_listino { get; set; }
        public string cod_imb { get; set; }
        public Nullable<double> altez_collo { get; set; }
        public Nullable<double> largh_collo { get; set; }
        public Nullable<double> profon_collo { get; set; }
        public string flag_conf_non_div { get; set; }
        public string flag_proddavend { get; set; }
        public Nullable<int> id_cat { get; set; }
        public string flag_contqua { get; set; }
        public string cod_causmag_ent { get; set; }
        public string flag_deperibile { get; set; }
        public string tipo_um_intra { get; set; }
        public string brochure { get; set; }
        public string gest_fasi { get; set; }
        public Nullable<double> peso_sgocciolato { get; set; }
        public Nullable<double> perc_calopeso_prod { get; set; }
        public Nullable<int> ind_fragilita { get; set; }
        public string tipo_art_ordforn { get; set; }
        public string flag_data_scad_par { get; set; }
        public string tipo_um_ordforn { get; set; }
        public string tipo_etichetta { get; set; }
        public Nullable<double> fatt_conv_intra { get; set; }
        public string cod_art_valoriz { get; set; }
        public string stato_autprg { get; set; }
        public string escludi_cat_web { get; set; }
        public string cod_com { get; set; }
        public string scheda { get; set; }
        public Nullable<int> giorni_dep { get; set; }
        public Nullable<System.DateTime> dataora_img_web { get; set; }
        public string cod_linea_prod_pre { get; set; }
        public Nullable<int> giorni_prod_ant { get; set; }
        public string cod_accisa { get; set; }
        public Nullable<double> grado_plato { get; set; }
        public string cod_contr { get; set; }
        public string cod_q_vino { get; set; }
        public Nullable<double> litri_conf { get; set; }
        public string cod_ass_prod_pre { get; set; }
        public string tipo_ean128 { get; set; }
        public string flag_servizio { get; set; }
        public string note_prenotazione { get; set; }
        public Nullable<double> qta_min_ordcli { get; set; }
        public string flag_qta_multipla { get; set; }
        public string ean_pezzo { get; set; }
        public string flag_gf { get; set; }
        public string etiamb_cod_imb { get; set; }
        public Nullable<double> peso_netto_pz { get; set; }
        public Nullable<double> tara_pz { get; set; }
        public string flag_stpda { get; set; }
        public string flag_stpla { get; set; }
        public string tipo_um_sm { get; set; }
        public string flag_no_qta { get; set; }
        public string flag_catalo_ok { get; set; }
        public string flag_bassa_margin { get; set; }
        public string pf_tipo_orig { get; set; }
        public string pf_luogo_orig { get; set; }
        public string flag_pf { get; set; }
        public string des_art_pers { get; set; }
        public string gest_fv { get; set; }
        public string flag_no_prz_man_pv { get; set; }
        public string flag_no_magg_prz { get; set; }
        public Nullable<double> seq_prelievo_dep2 { get; set; }
        public Nullable<int> prc_vita_res { get; set; }
        public string flag_tracc_partite { get; set; }
        public Nullable<int> qta_tipo_scad1 { get; set; }
        public string flag_bollo_cee { get; set; }
        public string pf_att_pesca { get; set; }
        public string flag_stpfrontalini { get; set; }
        public string flag_ean128_ct { get; set; }
        public Nullable<int> col_buonopre { get; set; }
        public Nullable<double> tassa1 { get; set; }
        public Nullable<double> tassa2 { get; set; }
        public Nullable<double> tassa3 { get; set; }
        public string tipo_um_stppre { get; set; }
        public string des_rep_man { get; set; }
        public Nullable<System.DateTime> data_blocco { get; set; }
        public Nullable<double> perc_glas { get; set; }
        public string des_art_completa { get; set; }
        public string cod_paese_orig { get; set; }
        public Nullable<System.DateTime> data_stato_fine { get; set; }
        public Nullable<int> mis_pezzi_collo { get; set; }
        public Nullable<double> mis_peso_netto_pez { get; set; }
        public Nullable<double> mis_peso_netto_col { get; set; }
        public Nullable<double> mis_peso_lordo_col { get; set; }
        public Nullable<double> mis_altez_pez { get; set; }
        public Nullable<double> mis_largh_pez { get; set; }
        public Nullable<double> mis_profon_pez { get; set; }
        public Nullable<double> mis_peso_lordo_pez { get; set; }
        public Nullable<int> mis_cluster_conf { get; set; }
        public string flag_erglink { get; set; }
        public string flag_in_esclusiva { get; set; }
        public string flag_one_shot { get; set; }
        public string flag_marketing { get; set; }
        public string flag_bloccato_ean_pezzo { get; set; }
        public string flag_bloccato_ean_art { get; set; }
        public string flag_bloccato_ean_cartone { get; set; }
        public Nullable<int> corsia { get; set; }
        public Nullable<int> colonna { get; set; }
        public string tppiano { get; set; }
    }
}
